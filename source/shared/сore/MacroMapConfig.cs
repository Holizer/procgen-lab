using Godot;
using ProcGenLab.Shared.Interfaces;

namespace ProcGenLab.Shared.Core;

[GlobalClass]
public partial class MacroMapConfig : GenerationConfig, IMapConfig
{
    [Export(PropertyHint.Range, "4, 32, 1")]
    public int MapWidthInChunks { get; set; } = 10;

    [Export(PropertyHint.Range, "4, 32, 1")]
    public int MapHeightInChunks { get; set; } = 10;

    [Export] public int MicroTilesPerMacroCell { get; set; } = 15;

    public int MacroCellSizePixels => MicroTilesPerMacroCell * TileSize;

    public int MapWidth => MapWidthInChunks;

    public int MapHeight => MapHeightInChunks;
}