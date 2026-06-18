using Godot;
using Godot.Collections;

namespace ProcGenLab.Shared.UI;

[Tool]
public partial class ScrollPanel : PanelContainer
{
    private string _titleText = "Title";

    [Export] public Label TitleLabel { get; set; }

    [Export] public Container ItemContainer { get; set; }

    [Export] public ScrollContainer ScrollNode { get; set; }

    [Export(PropertyHint.MultilineText)]
    public string TitleText
    {
        get => _titleText;
        set
        {
            _titleText = value;
            UpdateTitle();
        }
    }

    public override void _Ready()
    {
        UpdateTitle();
    }

    private void UpdateTitle()
    {
        if (TitleLabel != null) TitleLabel.Text = _titleText;
    }

    public void Clear()
    {
        if (ItemContainer == null)
            return;

        foreach (var child in ItemContainer.GetChildren()) child.QueueFree();
    }

    public void AddItem(Control item)
    {
        ItemContainer?.AddChild(item);
    }

    public Array<Node> GetItems()
    {
        return ItemContainer?.GetChildren() ?? new Array<Node>();
    }
}