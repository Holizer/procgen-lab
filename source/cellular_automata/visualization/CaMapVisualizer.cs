using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using ProcGenLab.CellularAutomata.Models;
using ProcGenLab.Shared.Enums;
using ProcGenLab.Shared.Utils;
using ProcGenLab.Shared.Visualization;

namespace ProcGenLab.CellularAutomata.Visualization;

public partial class CaMapVisualizer : BaseMapVisualizer<СaMapRenderContext>
{
    private TileLibrary _tileLibrary;

    [Export] public TileMapLayer DecorLayer;

    [Export] public int GroundTerrainId;

    [Export] public TileMapLayer TerrainLayer;

    [ExportGroup("Terrain Configuration")] [Export]
    public int TerrainSetId;

    [Export] public string TileTagLayerName = "tag";

    [Export] public int WaterTerrainId = 1;

    public void Initialize()
    {
        _tileLibrary = new TileLibrary(DecorLayer.TileSet, TileTagLayerName);
    }

    public override void Render(СaMapRenderContext context)
    {
        var map = context.CaMap;

        ClearAll();
        DrawTerrain(map);
        DrawProps(map);
    }

    private void DrawTerrain(CaMap map)
    {
        if (TerrainLayer == null)
            return;

        var width = map.Width;
        var height = map.Height;
        var totalCells = map.TotalCells;

        var groundCells = new List<Vector2I>(totalCells);
        var waterCells = new List<Vector2I>(totalCells);

        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            var idx = map.GetIndex(x, y);

            var pos = new Vector2I(x, y);
            waterCells.Add(pos);

            if (map.GetTerrain(idx) == TileType.Ground)
                groundCells.Add(pos);
        }

        TerrainLayer.SetCellsTerrainConnect(
            new Array<Vector2I>(waterCells),
            TerrainSetId,
            WaterTerrainId
        );

        TerrainLayer.SetCellsTerrainConnect(
            new Array<Vector2I>(groundCells),
            TerrainSetId,
            GroundTerrainId
        );
    }

    private void DrawProps(CaMap map)
    {
        if (DecorLayer == null || _tileLibrary == null)
            return;

        var width = map.Width;
        var height = map.Height;
        ReadOnlySpan<CaCell> gridSpan = map.Grid;

        for (var y = 0; y < height; y++)
        {
            var rowStartIndex = y * width;

            for (var x = 0; x < width; x++)
            {
                var currentIndex = rowStartIndex + x;
                ref readonly var cell = ref gridSpan[currentIndex];

                if (string.IsNullOrEmpty(cell.PropId))
                    continue;

                var pos = new Vector2I(x, y);

                if (_tileLibrary.HasTag(cell.PropId))
                    DrawTile(pos, cell.PropId);
                else
                    this.LogError(
                        $"Prop '{cell.PropId}' is present on the map but missing in TileSet!"
                    );
            }
        }
    }

    private void DrawTile(Vector2I pos, string propId)
    {
        var (atlasCoords, sourceId) = _tileLibrary.GetRandomTile(propId);
        if (atlasCoords != GridUtils.InvalidTile)
            DecorLayer.SetCell(pos, sourceId, atlasCoords);
    }

    private void ClearAll()
    {
        TerrainLayer?.Clear();
        DecorLayer?.Clear();
    }
}