using System.Runtime.CompilerServices;
using Godot;
using ProcGenLab.CellularAutomata.Enums;
using ProcGenLab.Shared.Core;
using ProcGenLab.Shared.Enums;
using ProcGenLab.Shared.Utils;

namespace ProcGenLab.CellularAutomata.Models;

public partial class CaMap : Grid2D<CaCell>
{
    private readonly int[] _distanceToWater;

    public CaMap(int width, int height)
        : base(width, height)
    {
        _distanceToWater = new int[TotalCells];
        for (var i = 0; i < Grid.Length; i++)
            Grid[i].Terrain = TileType.Water;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TileType GetTerrain(int index)
    {
        return Grid[index].Terrain;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BiomeZone? GetBiome(Vector2I coords)
    {
        return GridUtils.InBounds(coords, Width, Height) ? this[coords].Biome : null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetDistanceToWater(int x, int y)
    {
        if (!GridUtils.InBounds(x, y, Width, Height))
            return 0;
        return _distanceToWater[GetIndex(x, y)];
    }
}