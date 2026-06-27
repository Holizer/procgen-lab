using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Godot;

namespace ProcGenLab.Shared.Utils;

public static partial class GridUtils
{
    public static Vector2I GetCentroid(List<Vector2I> region)
    {
        return region == null ? Vector2I.Zero : GetCentroid(CollectionsMarshal.AsSpan(region));
    }

    public static Vector2I GetCentroid(ReadOnlySpan<Vector2I> region)
    {
        var count = region.Length;
        if (count == 0)
            return Vector2I.Zero;

        long sumX = 0;
        long sumY = 0;

        foreach (ref readonly var tile in region)
        {
            sumX += tile.X;
            sumY += tile.Y;
        }

        return new Vector2I(
            (int)Math.Round((double)sumX / count),
            (int)Math.Round((double)sumY / count)
        );
    }

    public static (Vector2I TileA, Vector2I TileB) FindClosestTiles(
        List<Vector2I> regionA,
        List<Vector2I> regionB
    )
    {
        var minDistanceSquare = int.MaxValue;

        var bestA = regionA[0];
        var bestB = regionB[0];

        foreach (var tileA in regionA)
        foreach (var tileB in regionB)
        {
            var distSquare = GetDistanceSquare(tileA, tileB);

            if (distSquare < minDistanceSquare)
            {
                minDistanceSquare = distSquare;
                bestA = tileA;
                bestB = tileB;
            }
        }

        return (bestA, bestB);
    }
}