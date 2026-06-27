using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using ProcGenLab.Shared.Enums;
using ProcGenLab.Shared.Utils;

namespace ProcGenLab.CellularAutomata.Models;

public partial class CaMap
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsType(int x, int y, TileType type)
    {
        if (!GridUtils.InBounds(x, y, Width, Height))
            return type == TileType.Water;
        return this[x, y].Terrain == type;
    }

    public bool IsRegionType(IReadOnlyList<Vector2I> region, TileType type)
    {
        if (region == null || region.Count == 0)
            return false;

        return IsType(region[0].X, region[0].Y, type);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsOccupied(int x, int y)
    {
        return !GridUtils.InBounds(x, y, Width, Height) || this[x, y].IsOccupied;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasAdjacentWater(Vector2I pos)
    {
        foreach (var dir in GridUtils.EightDirectionOffsets)
            if (IsType(pos.X + dir.X, pos.Y + dir.Y, TileType.Water))
                return true;
        return false;
    }

    public bool CanFitObject(
        Vector2I startPos,
        Vector2I size,
        TileType requiredTerrain,
        bool checkOccupied
    )
    {
        if (
            !GridUtils.InBounds(startPos.X, startPos.Y, Width, Height)
            || !GridUtils.InBounds(startPos.X + size.X - 1, startPos.Y + size.Y - 1, Width, Height)
        )
            return false;

        if (size is { X: 1, Y: 1 })
        {
            var idx = GetIndex(startPos);
            return Grid[idx].Terrain == requiredTerrain
                   && (!checkOccupied || !Grid[idx].IsOccupied);
        }

        var baseIdx = GetIndex(startPos);
        for (var y = 0; y < size.Y; y++)
        {
            var rowIdx = baseIdx + y * Width;
            for (var x = 0; x < size.X; x++)
            {
                var idx = rowIdx + x;
                if (Grid[idx].Terrain != requiredTerrain)
                    return false;
                if (checkOccupied && Grid[idx].IsOccupied)
                    return false;
            }
        }

        return true;
    }
}