using System;
using System.Collections.Generic;
using Godot;
using ProcGenLab.CellularAutomata.Enums;
using ProcGenLab.Shared.Enums;
using ProcGenLab.Shared.Utils;

namespace ProcGenLab.CellularAutomata.Models;

public partial class CaMap
{
    public void SetTile(int x, int y, TileType tileType)
    {
        if (!GridUtils.InBounds(x, y, Width, Height))
            return;

        Grid[GetIndex(x, y)].Terrain = tileType;
    }

    public void FillRegion(IEnumerable<Vector2I> region, TileType type)
    {
        if (region == null)
            return;

        foreach (var pos in region)
            SetTile(pos.X, pos.Y, type);
    }

    public void ApplyTerrain(ReadOnlySpan<TileType> source)
    {
        for (var i = 0; i < Grid.Length; i++)
            Grid[i].Terrain = source[i];
    }

    public void AssignBiome(Vector2I coords, BiomeZone biome)
    {
        if (GridUtils.InBounds(coords, Width, Height))
            this[coords].Biome = biome;
    }

    public void PlaceProp(Vector2I pos, string propId, Vector2I size)
    {
        if (!GridUtils.InBounds(pos.X, pos.Y, Width, Height))
            return;

        this[pos].PropId = propId;

        SetOccupiedArea(pos, size, true);
    }

    public void SetOccupiedArea(Vector2I startPos, Vector2I size, bool occupied)
    {
        if (!GridUtils.InBounds(startPos.X, startPos.Y, Width, Height))
            return;

        var baseIdx = GetIndex(startPos);

        for (var y = 0; y < size.Y; y++)
        {
            var rowIdx = baseIdx + y * Width;
            for (var x = 0; x < size.X; x++)
                if (GridUtils.InBounds(startPos.X + x, startPos.Y + y, Width, Height))
                    Grid[rowIdx + x].IsOccupied = occupied;
        }
    }
}