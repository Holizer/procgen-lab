using System;
using System.Runtime.CompilerServices;
using ProcGenLab.CellularAutomata.Models;
using ProcGenLab.Shared.Core;
using ProcGenLab.Shared.Enums;

namespace ProcGenLab.CellularAutomata.Services;

public class AutomataSimulator
{
    private TileType[] _buffer = [];

    public void RunStep(CaMap map, int wallThreshold)
    {
        var (width, height, total) = map;

        if (_buffer.Length != total)
            _buffer = new TileType[total];

        ReadOnlySpan<CaCell> current = map.Grid;
        var next = _buffer.AsSpan(0, total);

        for (var y = 0; y < height; y++)
        {
            var rowOffset = y * width;

            for (var x = 0; x < width; x++)
            {
                var walls = GetSurroundingWallCount(x, y, width, height, current);
                var idx = rowOffset + x;

                if (walls > wallThreshold)
                    next[idx] = TileType.Water;
                else if (walls < wallThreshold)
                    next[idx] = TileType.Ground;
                else
                    next[idx] = current[idx].Terrain;
            }
        }

        map.ApplyTerrain(next);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetSurroundingWallCount(
        int gridX,
        int gridY,
        int width,
        int height,
        ReadOnlySpan<CaCell> grid
    )
    {
        var wallCount = 0;

        for (var ny = gridY - 1; ny <= gridY + 1; ny++)
        {
            if (ny < 0 || ny >= height)
            {
                wallCount += 3;

                continue;
            }

            var row = ny * width;

            for (var nx = gridX - 1; nx <= gridX + 1; nx++)
            {
                if (nx == gridX && ny == gridY)
                    continue;

                if (nx < 0 || nx >= width)
                    wallCount++;
                else if (grid[row + nx].Terrain == TileType.Water)
                    wallCount++;
            }
        }

        return wallCount;
    }
}