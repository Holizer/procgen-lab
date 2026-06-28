using Godot;

namespace ProcGenLab.Shared.UI;

[Tool]
public partial class ConfigRow : Control
{
    public override void _Ready()
    {
        UpdateVisibility();
        UpdateLabel();

        if (Engine.IsEditorHint())
            return;

        MouseFilter = MouseFilterEnum.Pass;
        MouseEntered += OnMouseEnteredRow;
        MouseExited += OnMouseExitedRow;

        if (HoverBackground != null)
            HoverBackground.Visible = false;

        if (LabelNode != null)
        {
            LabelNode.MouseFilter = MouseFilterEnum.Pass;
            LabelNode.MouseEntered += OnMouseEnteredRow;
            LabelNode.MouseExited += OnMouseExitedRow;
        }

        if (SpinBoxNode != null)
        {
            SpinBoxNode.MouseEntered += OnMouseEnteredRow;
            SpinBoxNode.MouseExited += OnMouseExitedRow;

            var internalLineEdit = SpinBoxNode.GetLineEdit();

            if (internalLineEdit != null)
            {
                internalLineEdit.TextChanged += OnTextChanged;
                internalLineEdit.MouseEntered += OnMouseEnteredRow;
                internalLineEdit.MouseExited += OnMouseExitedRow;

                internalLineEdit.FocusEntered += OnFocusEnteredInput;
                internalLineEdit.FocusExited += OnFocusExitedInput;
            }
        }

        if (CheckBoxNode != null)
        {
            CheckBoxNode.MouseEntered += OnMouseEnteredRow;
            CheckBoxNode.MouseExited += OnMouseExitedRow;

            CheckBoxNode.FocusEntered += OnFocusEnteredInput;
            CheckBoxNode.FocusExited += OnFocusExitedInput;
        }
    }

    private void UpdateLabel()
    {
        if (LabelNode != null)
        {
            LabelNode.Text = _labelText;
            LabelNode.MouseFilter = string.IsNullOrWhiteSpace(_labelTooltip)
                ? MouseFilterEnum.Ignore
                : MouseFilterEnum.Stop;
        }
    }

    private void UpdateVisibility()
    {
        if (SpinBoxNode != null)
            SpinBoxNode.Visible = _type == ConfigRowVariants.LineEdit;

        if (CheckBoxNode != null)
            CheckBoxNode.Visible = _type == ConfigRowVariants.CheckBox;
    }

    private void HighlightRow(bool enabled)
    {
        if (HoverBackground != null)
            HoverBackground.Visible = enabled;
    }

    public void Setup(
        string title,
        double value,
        string suffix = "",
        double min = 0.0,
        double max = 100.0,
        double step = 1,
        string tooltip = ""
    )
    {
        Type = ConfigRowVariants.LineEdit;
        LabelText = title;
        LabelTooltip = tooltip;

        if (SpinBoxNode != null)
        {
            SpinBoxNode.MinValue = min;
            SpinBoxNode.MaxValue = max;
            SpinBoxNode.Suffix = suffix;
            SpinBoxNode.Step = step;
        }

        Value = value;
    }

    public void Setup(string title, bool isChecked, string tooltip = "")
    {
        Type = ConfigRowVariants.CheckBox;
        LabelText = title;
        LabelTooltip = tooltip;
        IsChecked = isChecked;
    }
}