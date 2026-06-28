using Godot;
using Godot.Collections;
using ProcGenLab.BSP.Models;
using ProcGenLab.Shared.Visualization;

namespace ProcGenLab.BSP.Visualization;

public partial class BspMapVisualizer : BaseMapVisualizer<BspMapRenderContext>
{
    [ExportGroup("Terrain Configuration")]
    [Export]
    public int FloorTerrainId { get; set; }

    [Export] public int TerrainSetId { get; set; }

    [Export] public int WallTerrainId { get; set; } = 1;

    [Export] public TileMapLayer TerrainLayer { get; set; } = null!;

    [ExportGroup("Edge Configuration")]
    [Export]
    public Vector2I SolidTileCoords { get; set; }

    [Export] public int SolidTileSourceId { get; set; }

    private void DrawTerrain(BspMap map)
    {
        if (TerrainLayer == null)
            return;

        var floorCells = new Array<Vector2I>(map.GetFloorTiles());

        TerrainLayer.SetCellsTerrainConnect(floorCells, TerrainSetId, FloorTerrainId, false);

        foreach (var cell in floorCells)
            if (TerrainLayer.GetCellSourceId(cell) == -1)
                TerrainLayer.SetCell(cell, SolidTileSourceId, SolidTileCoords);
    }
}