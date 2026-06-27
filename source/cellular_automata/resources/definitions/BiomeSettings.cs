using Godot;
using Godot.Collections;
using ProcGenLab.CellularAutomata.Enums;

namespace ProcGenLab.CellularAutomata.Resources;

[GlobalClass]
public partial class BiomeSettings : Resource
{
    [Export] public BiomeZone Type { get; set; }

    [Export(PropertyHint.Range, "0, 100, 1, suffix:%")]
    public float PropDensity { get; set; } = 30f;

    [Export] public Array<PropRule> PropRules { get; set; } = new();

    public float PropDensityNormalized => PropDensity / 100f;
}