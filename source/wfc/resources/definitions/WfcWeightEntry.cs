using Godot;
using ProcGenLab.WFC.Enums;

namespace ProcGenLab.WFC.Resources;

[GlobalClass]
public partial class WfcWeightEntry : Resource
{
    [Export] public MacroTileType Type { get; set; }

    [Export] public float Weight { get; set; } = 1.0f;
}