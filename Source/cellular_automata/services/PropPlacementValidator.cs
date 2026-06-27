using Godot;
using ProcGenLab.CellularAutomata.Enums;
using ProcGenLab.CellularAutomata.Models;
using ProcGenLab.Shared.Enums;

namespace ProcGenLab.CellularAutomata.Services;

public static class PropPlacementValidator
{
    public static bool IsValidContextForArea(
        CaMap map,
        Vector2I startPos,
        Vector2I size,
        PlacementContext context
    )
    {
        if (!IsValidContext(map, startPos, context)) return false;

        if (!IsValidContext(map, new Vector2I(startPos.X + size.X - 1, startPos.Y), context)) return false;

        if (!IsValidContext(map, new Vector2I(startPos.X, startPos.Y + size.Y - 1), context)) return false;

        if (!IsValidContext(map, new Vector2I(startPos.X + size.X - 1, startPos.Y + size.Y - 1), context)) return false;

        return true;
    }

    private static bool IsValidContext(CaMap map, Vector2I pos, PlacementContext context)
    {
        var biome = map.GetBiome(pos);
        return context switch
        {
            PlacementContext.LakeOnly => biome == BiomeZone.Lake,
            PlacementContext.OceanOnly => biome == BiomeZone.Sea,

            PlacementContext.GroundOnly => map.IsType(pos.X, pos.Y, TileType.Ground)
                                           && !map.HasAdjacentWater(pos),
            _ => true
        };
    }
}