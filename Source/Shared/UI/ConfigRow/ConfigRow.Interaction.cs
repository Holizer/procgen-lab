using Godot;
using ProcGenLab.Shared.Utils;

namespace ProcGenLab.Shared.UI;

public partial class ConfigRow
{
    [Signal]
    public delegate void RowHoveredEventHandler(string description);

    [Signal]
    public delegate void RowUnhoveredEventHandler();

    private bool _isFocused;

    private bool _isHovered;

    private void OnMouseEnteredRow()
    {
        TooltipBus.SetHover(LabelTooltip);
        HighlightRow(true);
    }

    private void OnMouseExitedRow()
    {
        TooltipBus.ClearHover();
        if (!_isFocused)
            HighlightRow(false);
    }

    private void OnFocusEnteredInput()
    {
        _isFocused = true;
        TooltipBus.SetFocus(LabelTooltip);
        HighlightRow(true);

        if (SpinBoxNode != null)
        {
            var lineEdit = SpinBoxNode.GetLineEdit();
            if (lineEdit != null && lineEdit.HasThemeColor("font_focus_color"))
            {
                var focusColor = lineEdit.GetThemeColor("font_focus_color");
                lineEdit.AddThemeColorOverride("font_color", focusColor);
            }
        }
    }

    private void OnFocusExitedInput()
    {
        _isFocused = false;
        TooltipBus.ClearFocus();
        HighlightRow(false);

        if (SpinBoxNode != null)
        {
            var lineEdit = SpinBoxNode.GetLineEdit();
            if (lineEdit != null)
                lineEdit.RemoveThemeColorOverride("font_color");
        }
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (Engine.IsEditorHint())
            return;

        if (
            @event is InputEventMouseButton mouseEvent
            && mouseEvent.Pressed
            && mouseEvent.ButtonIndex == MouseButton.Left
        )
            FocusInput();
    }

    private void FocusInput()
    {
        if (_type == ConfigRowVariants.LineEdit && SpinBoxNode != null)
        {
            SpinBoxNode.GetLineEdit().GrabFocus();
        }
        else if (_type == ConfigRowVariants.CheckBox && CheckBoxNode != null)
        {
            CheckBoxNode.GrabFocus();
            CheckBoxNode.ButtonPressed = !CheckBoxNode.ButtonPressed;
        }
    }

    private void OnTextChanged(string newText)
    {
        if (SpinBoxNode == null)
            return;

        SpinBoxNode.GetLineEdit().FilterNumericInput(newText);
    }
}