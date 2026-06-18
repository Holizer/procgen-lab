using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using ProcGenLab.CellularAutomata.Enums;
using ProcGenLab.Shared.Enums;
using ProcGenLab.Shared.Utils;

namespace ProcGenLab.CellularAutomata.Models;

public partial class CaMap
{
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetTile(int x, int y, TileType tileType)
    {
        if (!GridUtils.InBounds(x, y, Width, Height))
            return;

        Grid[GetIndex(x, y)].Terrain = tileType;
    }

    public void PlaceProp(Vector2I pos, string propId, Vector2I size)
    {
        this[pos].PropId = propId;
        var baseIdx = GetIndex(pos);
        for (var y = 0; y < size.Y; y++)
        {
            var rowIdx = baseIdx + y * Width;
            for (var x = 0; x < size.X; x++)
                Grid[rowIdx + x].IsOccupied = true;
        }
    }

    public void GenerateDistanceMaps()
    {
        var queue = new Queue<int>(TotalCells);

        for (var i = 0; i < TotalCells; i++)
            if (Grid[i].Terrain == TileType.Water)
            {
                _distanceToWater[i] = 0;
                queue.Enqueue(i);
            }
            else
            {
                _distanceToWater[i] = int.MaxValue;
            }

        while (queue.Count > 0)
        {
            var currentIdx = queue.Dequeue();
            var currentDist = _distanceToWater[currentIdx];

            var cx = currentIdx % Width;
            var cy = currentIdx / Width;

            foreach (var dir in GridUtils.DirectionOffsets)
            {
                Vector2I neighborPos = new(cx + dir.X, cy + dir.Y);
                if (GridUtils.InBounds(neighborPos, Width, Height))
                {
                    var neighborIdx = GetIndex(neighborPos.X, neighborPos.Y);

                    if (_distanceToWater[neighborIdx] > currentDist + 1)
                    {
                        _distanceToWater[neighborIdx] = currentDist + 1;
                        queue.Enqueue(neighborIdx);
                    }
                }
            }
        }
    }
}