using Godot;
using ProcGenLab.Shared.Core;
using ProcGenLab.Shared.Interfaces;

namespace ProcGenLab.WFC.Resources;

[GlobalClass]
public partial class WfcConfig : MacroMapConfig, IBspConfig
{
    [ExportGroup("Generation Mode")]
    [Export]
    public bool UseBspTopology { get; set; } = true;

    [Export] public WfcWeightConfig Weights { get; set; }

    [ExportGroup("BSP Constraints")]
    [Export(PropertyHint.Range, "1, 20, 1")]
    public int MaxDepth { get; set; } = 4;

    [Export(PropertyHint.Range, "1.0, 3.0, 0.05")]
    public float AspectRatioThreshold { get; set; } = 1.5f;

    [Export(PropertyHint.Range, "1, 10, 1")]
    public int MinSplitSize { get; set; } = 4;
}