using Godot;
using Godot.Collections;
using ProcGenLab.Shared.Core;

namespace ProcGenLab.CellularAutomata.Resources;

[GlobalClass]
public partial class CaConfig : PixelMapConfig
{
    [ExportGroup("Biomes Settings")]
    [Export]
    public Array<BiomeSettings> BiomesSettings { get; set; }

    [Export(PropertyHint.Range, "0.005,0.08,0.001")]
    public float BiomeNoiseFrequency { get; set; } = 0.03f;

    [ExportGroup("Props Database")]
    [Export]
    public PropDatabase PropDatabase { get; set; }

    [ExportGroup("CA Rules")]
    [Export(PropertyHint.Range, "0, 100, 1")]
    public int FillPercent { get; set; } = 45;

    [Export(PropertyHint.Range, "2, 5, 1")]
    public int WallTransitionThreshold { get; set; } = 4;

    [Export(PropertyHint.Range, "0, 10, 1")]
    public int SimulationSteps { get; set; } = 5;

    [ExportGroup("Cleanup Settings")]
    [Export(PropertyHint.Range, "0, 500, 1")]

    public int MinIslandSizeTiles { get; set; } = 10;

    [Export(PropertyHint.Range, "0, 500, 1")]

    public int MinLakeSizeTiles { get; set; } = 50;
}