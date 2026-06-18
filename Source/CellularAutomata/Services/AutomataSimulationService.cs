using System;
using System.Runtime.CompilerServices;
using ProcGenLab.CellularAutomata.Models;
using ProcGenLab.CellularAutomata.Resources;
using ProcGenLab.Shared.Enums;

namespace ProcGenLab.CellularAutomata.Services;

public class AutomataSimulationService
{
    private TileType[] _buffer = Array.Empty<TileType>();

    public void RunStep(CaMap map, CaConfig config)
    {
        var width = map.Width;
        var height = map.Height;
        var totalCells = width * height;
        var maxWalls = config.MaxSurroundingWalls;

        if (_buffer.Length != totalCells)
            _buffer = new TileType[totalCells];

        ReadOnlySpan<CaCell> current = map.Grid;
        var next = _buffer.AsSpan(0, totalCells);

        for (var y = 0; y < height; y++)
        {
            var row = y * width;
            for (var x = 0; x < width; x++)
            {
                var walls = GetSurroundingWallCount(x, y, width, height, current);
                var idx = row + x;

                if (walls > maxWalls)
                    next[idx] = TileType.Water;
                else if (walls < maxWalls)
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