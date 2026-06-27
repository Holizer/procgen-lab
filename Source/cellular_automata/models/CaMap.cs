using System.Runtime.CompilerServices;
using Godot;
using ProcGenLab.CellularAutomata.Enums;
using ProcGenLab.Shared.Core;
using ProcGenLab.Shared.Enums;
using ProcGenLab.Shared.Utils;

namespace ProcGenLab.CellularAutomata.Models;

public partial class CaMap : Grid2D<CaCell>
{
    public CaMap(int width, int height)
        : base(width, height)
    {
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
}