using System;
using System.Collections.Generic;
using Godot;
using ProcGenLab.CellularAutomata.Models;
using ProcGenLab.Shared.Enums;
using ProcGenLab.Shared.Utils;

namespace ProcGenLab.CellularAutomata.Services;

public class RegionConnector
{
    public void Connect(CaMap map, List<List<Vector2I>> regions)
    {
        if (regions.Count < 2)
            return;

        var centroids = new Vector2I[regions.Count];
        for (var i = 0; i < regions.Count; i++)
            centroids[i] = GridUtils.GetCentroid(regions[i]);

        var connectedRegions = new List<int> { 0 };
        var unconnectedRegions = new List<int>();
        for (var i = 1; i < regions.Count; i++)
            unconnectedRegions.Add(i);

        while (unconnectedRegions.Count > 0)
        {
            var minDistanceSquare = int.MaxValue;
            var bestConnectedIdx = -1;
            var bestUnconnectedIdx = -1;

            foreach (var cIdx in connectedRegions)
            foreach (var uIdx in unconnectedRegions)
            {
                var distSq = GridUtils.GetDistanceSquare(centroids[cIdx], centroids[uIdx]);

                if (distSq < minDistanceSquare)
                {
                    minDistanceSquare = distSq;
                    bestConnectedIdx = cIdx;
                    bestUnconnectedIdx = uIdx;
                }
            }

            var (tileA, tileB) = GridUtils.FindClosestTiles(
                regions[bestConnectedIdx],
                regions[bestUnconnectedIdx]
            );

            BridgeTilesWithTunnel(map, tileA, tileB);

            connectedRegions.Add(bestUnconnectedIdx);
            unconnectedRegions.RemoveAt(unconnectedRegions.FindIndex(x => x == bestUnconnectedIdx));
        }
    }

    private static void BridgeTilesWithTunnel(CaMap map, Vector2I start, Vector2I end)
    {
        var x = start.X;
        var y = start.Y;

        var dx = Math.Abs(end.X - start.X);
        var dy = Math.Abs(end.Y - start.Y);

        var stepX = start.X < end.X ? 1 : -1;
        var stepY = start.Y < end.Y ? 1 : -1;

        var error = dx - dy;

        while (true)
        {
            DigGroundBrush(map, x, y, 1);

            if (x == end.X && y == end.Y)
                break;

            var error2 = 2 * error;

            if (error2 > -dy)
            {
                error -= dy;
                x += stepX;
            }

            if (error2 < dx)
            {
                error += dx;
                y += stepY;
            }
        }
    }

    private static void DigGroundBrush(CaMap map, int centerX, int centerY, int radius)
    {
        for (var kx = -radius; kx <= radius; kx++)
        for (var ky = -radius; ky <= radius; ky++)
        {
            var targetX = centerX + kx;
            var targetY = centerY + ky;

            if (
                GridUtils.InBounds(targetX, targetY, map.Width, map.Height)
                && map.IsType(targetX, targetY, TileType.Water)
            )
                map.SetTile(targetX, targetY, TileType.Ground);
        }
    }
}