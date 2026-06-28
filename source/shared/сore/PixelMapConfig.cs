using Godot;
using ProcGenLab.Shared.Interfaces;

namespace ProcGenLab.Shared.Core;

[GlobalClass]
public partial class PixelMapConfig : GenerationConfig, IMapConfig
{
    [ExportGroup("Map Dimensions")]
    [Export(PropertyHint.Range, "512, 2048, 1")]
    public int MapWidthPx { get; set; } = 1024;

    [Export(PropertyHint.Range, "512, 2048, 1")]
    public int MapHeightPx { get; set; } = 1024;

    public int MapWidth => FromPxToTiles(MapWidthPx);

    public int MapHeight => FromPxToTiles(MapHeightPx);

    protected int FromPxToTiles(float px)
    {
        var size = TileSize > 0 ? TileSize : 16;

        return Mathf.CeilToInt(px / size);
    }
}