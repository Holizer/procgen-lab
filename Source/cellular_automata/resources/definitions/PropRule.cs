using Godot;
using ProcGenLab.CellularAutomata.Enums;

namespace ProcGenLab.CellularAutomata.Resources;

[GlobalClass]
public partial class PropRule : Resource
{
    [Export] public PropType Type { get; set; }

    [Export(PropertyHint.Range, "1, 100, 1")]
    public int Weight { get; set; } = 10;

    [Export] public PlacementContext Context { get; set; } = PlacementContext.GroundOnly;
}