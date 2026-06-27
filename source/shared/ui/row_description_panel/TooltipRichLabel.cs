using Godot;

namespace ProcGenLab.Shared.UI;

[Tool]
public partial class TooltipRichLabel : RichTextLabel
{
    [Export] public string UnhoveredLabelText { get; set; } = "Hover over the parameter for a tooltip...";

    public override void _Ready()
    {
        BbcodeEnabled = true;

        Text = UnhoveredLabelText;

        if (Engine.IsEditorHint())
            return;

        TooltipBus.OnRowHovered += UpdateText;
        TooltipBus.OnRowUnhovered += ResetText;
    }

    public override void _ExitTree()
    {
        if (Engine.IsEditorHint())
            return;

        TooltipBus.OnRowHovered -= UpdateText;
        TooltipBus.OnRowUnhovered -= ResetText;
    }

    private void UpdateText(string desc)
    {
        Text = desc;
    }

    private void ResetText()
    {
        Text = UnhoveredLabelText;
    }
}