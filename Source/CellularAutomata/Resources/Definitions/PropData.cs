using Godot;

namespace ProcGenLab.CellularAutomata.Resources;

[GlobalClass]
public partial class PropData : Resource
{
    [Export] public string Tag { get; set; }

    [Export] public Vector2I ObjectSize { get; set; } = Vector2I.One;

    [Export(PropertyHint.Range, "1, 100, 1")]
    public int Weight { get; set; } = 10;
}