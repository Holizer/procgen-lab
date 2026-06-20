using System.Collections.Generic;
using Godot;
using ProcGenLab.CellularAutomata.Models;
using ProcGenLab.Shared.Core;
using ProcGenLab.Shared.Enums;

namespace ProcGenLab.CellularAutomata.Services;

public class RegionAnalyzer
{
    private const int InitialRegionCapacity = 256;

    public List<List<Vector2I>> GetAllRegions(CaMap map)
    {
        var (width, height, total) = map;

        var visited = new bool[total];
        var regions = new List<List<Vector2I>>();

        for (var y = 0; y < height; y++)
        {
            var row = map.GetRow(y);
            var rowOffset = y * width;

            for (var x = 0; x < width; x++)
            {
                var idx = rowOffset + x;

                if (visited[idx])
                    continue;

                var type = row[x].Terrain;
                regions.Add(FloodFill(map, x, y, type, width, height, visited));
            }
        }

        return regions;
    }

    public List<List<Vector2I>> GetRegionsType(CaMap map, TileType targetType)
    {
        var (width, height, total) = map;

        var visited = new bool[total];
        var regions = new List<List<Vector2I>>();

        for (var y = 0; y < height; y++)
        {
            var row = map.GetRow(y);
            var rowOffset = y * width;

            for (var x = 0; x < width; x++)
            {
                var idx = rowOffset + x;

                if (!visited[idx] && row[x].Terrain == targetType)
                    regions.Add(FloodFill(map, x, y, targetType, width, height, visited));
            }
        }

        return regions;
    }

    private List<Vector2I> FloodFill(
        CaMap map,
        int startX,
        int startY,
        TileType type,
        int width,
        int height,
        bool[] visited
    )
    {
        var tiles = new List<Vector2I>(InitialRegionCapacity);
        var queue = new Queue<int>(512);

        var startIdx = startX + startY * width;
        visited[startIdx] = true;
        queue.Enqueue(startIdx);

        while (queue.Count > 0)
        {
            var idx = queue.Dequeue();
            var cx = idx % width;
            var cy = idx / width;

            tiles.Add(new Vector2I(cx, cy));

            if (cy > 0)
            {
                var ni = idx - width;
                if (!visited[ni] && map.GetTerrain(ni) == type)
                {
                    visited[ni] = true;
                    queue.Enqueue(ni);
                }
            }

            if (cy < height - 1)
            {
                var ni = idx + width;
                if (!visited[ni] && map.GetTerrain(ni) == type)
                {
                    visited[ni] = true;
                    queue.Enqueue(ni);
                }
            }

            if (cx > 0)
            {
                var ni = idx - 1;
                if (!visited[ni] && map.GetTerrain(ni) == type)
                {
                    visited[ni] = true;
                    queue.Enqueue(ni);
                }
            }

            if (cx < width - 1)
            {
                var ni = idx + 1;
                if (!visited[ni] && map.GetTerrain(ni) == type)
                {
                    visited[ni] = true;
                    queue.Enqueue(ni);
                }
            }
        }

        return tiles;
    }

    public void CleanupRegions(CaMap map, int minGroundSize, int minWaterSize)
    {
        var allRegions = GetAllRegions(map);

        foreach (var region in allRegions)
        {
            if (region.Count == 0)
                continue;

            var firstCellPos = region[0];
            var type = map.GetTerrain(map.GetIndex(firstCellPos.X, firstCellPos.Y));
            var isGround = type == TileType.Ground;

            if (isGround && region.Count < minGroundSize)
                map.FillRegion(region, TileType.Water);
            else if (!isGround && region.Count < minWaterSize)
                map.FillRegion(region, TileType.Ground);
        }
    }
}