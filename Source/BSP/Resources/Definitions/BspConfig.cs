using Godot;
using ProcGenLab.Shared.Core;
using ProcGenLab.Shared.Interfaces;

namespace ProcGenLab.BSP.Resources;

[GlobalClass]
public partial class BspConfig : PixelMapConfig, IBspConfig
{
    [ExportGroup("Enemies")] [Export] public BspEnemyConfig EnemyConfig { get; set; }

    [ExportGroup("Quantities")]
    [Export(PropertyHint.Range, "0, 99, 1")]
    public int MaxTreasureRooms { get; set; } = 2;

    [ExportGroup("Room Sizes (Pixels)")]
    [Export(PropertyHint.Range, "48, 512, 1")]
    public int MinRoomWidthPx { get; set; } = 48;

    [Export(PropertyHint.Range, "48, 512, 1")]
    public int MinRoomHeightPx { get; set; } = 48;

    [Export(PropertyHint.Range, "32, 64, 1")]
    public int RoomPaddingPx { get; set; } = 16;

    public int MinRoomWidthTiles => FromPxToTiles(MinRoomWidthPx);
    public int MinRoomHeightTiles => FromPxToTiles(MinRoomHeightPx);
    public int PaddingTiles => FromPxToTiles(RoomPaddingPx);
    public int MinSplitSize => Mathf.Max(MinRoomWidthTiles, MinRoomHeightTiles) + PaddingTiles * 2;

    [ExportGroup("BSP Constraints")]
    [Export(PropertyHint.Range, "1, 20, 1")]
    public int MaxDepth { get; set; } = 4;

    [Export(PropertyHint.Range, "1.0, 3.0, 0.05")]
    public float AspectRatioThreshold { get; set; } = 1.25f;
}