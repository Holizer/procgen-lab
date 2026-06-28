using Godot;

namespace ProcGenLab.Shared.UI;

public partial class ConfigRow
{
    public enum ConfigRowVariants
    {
        LineEdit = 0,

        CheckBox = 1
    }

    private string _labelText = "Parameter";

    private string _labelTooltip = "";

    private ConfigRowVariants _type;

    [Export]
    public string LabelText
    {
        get => _labelText;
        set
        {
            _labelText = value;
            UpdateLabel();
        }
    }

    [Export(PropertyHint.MultilineText)]
    public string LabelTooltip
    {
        get => _labelTooltip;
        set
        {
            _labelTooltip = value;
            UpdateLabel();
        }
    }

    [Export]
    public ConfigRowVariants Type
    {
        get => _type;
        set
        {
            _type = value;
            UpdateVisibility();
            NotifyPropertyListChanged();
        }
    }

    [ExportGroup("Internal Nodes")]
    [Export]
    public SpinBox SpinBoxNode { get; set; }

    [Export] public Label LabelNode { get; set; }

    [Export] public CheckBox CheckBoxNode { get; set; }

    public double Value
    {
        get => SpinBoxNode?.Value ?? 0.0;
        set
        {
            if (SpinBoxNode != null)
                SpinBoxNode.Value = value;
        }
    }

    public bool IsChecked
    {
        get => CheckBoxNode?.ButtonPressed ?? false;
        set
        {
            if (CheckBoxNode != null)
                CheckBoxNode.ButtonPressed = value;
        }
    }

    [Export] public Control HoverBackground { get; set; }
}