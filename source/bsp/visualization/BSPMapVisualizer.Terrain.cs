using Godot;
using Godot.Collections;
using ProcGenLab.BSP.Models;
using ProcGenLab.Shared.Enums;
using ProcGenLab.Shared.Utils;
using ProcGenLab.Shared.Visualization;

namespace ProcGenLab.BSP.Visualization;

public partial class BspMapVisualizer : BaseMapVisualizer<BspMapRenderContext>
{
    [Export] public int FloorTerrainId;

    [Export] public int TerrainSetId;

    [Export] public int WallTerrainId = 1;

    [ExportGroup("Terrain Configuration")]
    [Export]
    public TileMapLayer TerrainLayer { get; set; } = null!;

    [ExportGroup("Edge Configuration")]
    [Export]
    public Vector2I SolidTileCoords { get; set; }

    [Export] public int SolidTileSourceId { get; set; }

    private void DrawTerrain(BspMap map)
    {
        if (TerrainLayer == null)
            return;

        var width = map.Width;
        var height = map.Height;

        var wallCells = new Array<Vector2I>();
        var floorCells = new Array<Vector2I>();

        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            var pos = new Vector2I(x, y);
            if (map.Grid[map.GetIndex(x, y)] == TileType.Wall)
                wallCells.Add(pos);
            else if (map.Grid[map.GetIndex(x, y)] == TileType.Floor)
                floorCells.Add(pos);
        }

        TerrainLayer.SetCellsTerrainConnect(
            wallCells,
            TerrainSetId,
            WallTerrainId,
            false
        );
        TerrainLayer.SetCellsTerrainConnect(
            floorCells,
            TerrainSetId,
            FloorTerrainId,
            false
        );
    }

    private void DrawTerrainSolidBorder(BspMap map)
    {
        if (TerrainLayer == null)
            return;

        for (var y = 0; y < map.Height; y++)
        for (var x = 0; x < map.Width; x++)
        {
            if (!GridUtils.IsBorder(x, y, map.Width, map.Height))
                continue;

            TerrainLayer.SetCell(new Vector2I(x, y), SolidTileSourceId, SolidTileCoords);
        }
    }
}