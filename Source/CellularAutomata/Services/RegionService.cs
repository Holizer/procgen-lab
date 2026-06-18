using System;
using System.Collections.Generic;
using Godot;
using ProcGenLab.CellularAutomata.Models;
using ProcGenLab.Shared.Enums;

namespace ProcGenLab.CellularAutomata.Services;

public class RegionService
{
    private const int InitialRegionCapacity = 256;
    private readonly Queue<int> _queue = new(512);

    private bool[] _visited = Array.Empty<bool>();

    private void PrepareVisitedBuffer(int size)
    {
        if (_visited.Length != size)
            _visited = new bool[size];
        else
            Array.Clear(_visited, 0, _visited.Length);
    }

    public List<List<Vector2I>> GetAllRegions(CaMap map)
    {
        var width = map.Width;
        var height = map.Height;
        var total = width * height;

        PrepareVisitedBuffer(total);

        var regions = new List<List<Vector2I>>();

        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            var idx = map.GetIndex(x, y);
            if (!_visited[idx])
            {
                var type = map.GetTerrain(idx);
                regions.Add(FloodFill(map, x, y, type, width, height));
            }
        }

        return regions;
    }

    public List<List<Vector2I>> GetRegionsType(CaMap map, TileType targetType)
    {
        var width = map.Width;
        var height = map.Height;

        PrepareVisitedBuffer(width * height);

        var regions = new List<List<Vector2I>>();

        for (var y = 0; y < height; y++)
        {
            var row = y * width;
            for (var x = 0; x < width; x++)
            {
                var idx = row + x;
                if (!_visited[idx] && map.IsType(x, y, targetType))
                    regions.Add(FloodFill(map, x, y, targetType, width, height));
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
        int height
    )
    {
        var tiles = new List<Vector2I>(InitialRegionCapacity);

        _queue.Clear();

        var startIdx = startX + startY * width;
        _visited[startIdx] = true;
        _queue.Enqueue(startIdx);

        while (_queue.Count > 0)
        {
            var idx = _queue.Dequeue();
            var cx = idx % width;
            var cy = idx / width;

            tiles.Add(new Vector2I(cx, cy));

            if (cy > 0)
            {
                var ni = idx - width;
                if (!_visited[ni] && map.IsType(cx, cy - 1, type))
                {
                    _visited[ni] = true;
                    _queue.Enqueue(ni);
                }
            }

            if (cy < height - 1)
            {
                var ni = idx + width;
                if (!_visited[ni] && map.IsType(cx, cy + 1, type))
                {
                    _visited[ni] = true;
                    _queue.Enqueue(ni);
                }
            }

            if (cx > 0)
            {
                var ni = idx - 1;
                if (!_visited[ni] && map.IsType(cx - 1, cy, type))
                {
                    _visited[ni] = true;
                    _queue.Enqueue(ni);
                }
            }

            if (cx < width - 1)
            {
                var ni = idx + 1;
                if (!_visited[ni] && map.IsType(cx + 1, cy, type))
                {
                    _visited[ni] = true;
                    _queue.Enqueue(ni);
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
            var type = map.GetTerrain(map.GetIndex(firstCellPos));
            var isGround = type == TileType.Ground;

            if (isGround && region.Count < minGroundSize)
                FillRegion(map, region, TileType.Water);
            else if (!isGround && region.Count < minWaterSize)
                FillRegion(map, region, TileType.Ground);
        }
    }

    public void FillRegion(CaMap map, IEnumerable<Vector2I> region, TileType type)
    {
        foreach (var tile in region)
            map.SetTile(tile.X, tile.Y, type);
    }
}