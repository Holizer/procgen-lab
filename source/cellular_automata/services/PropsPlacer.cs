using System.Collections.Generic;
using Godot;
using ProcGenLab.CellularAutomata.Enums;
using ProcGenLab.CellularAutomata.Models;
using ProcGenLab.CellularAutomata.Resources;
using ProcGenLab.Shared.Enums;
using ProcGenLab.Shared.Utils;

namespace ProcGenLab.CellularAutomata.Services;

public class PropsPlacer(PropDatabase db)
{
    public void PlaceProps(
        CaMap map,
        List<List<Vector2I>> allRegions,
        IReadOnlyDictionary<BiomeZone, BiomeSettings> biomesMap,
        RandomNumberGenerator rng
    )
    {
        foreach (var region in allRegions)
        {
            if (region == null || region.Count == 0) continue;

            foreach (var pos in region)
                TryPlacePropInCell(map, pos, biomesMap, rng);
        }
    }

    private void TryPlacePropInCell(
        CaMap map,
        Vector2I pos,
        IReadOnlyDictionary<BiomeZone, BiomeSettings> biomesMap,
        RandomNumberGenerator rng
    )
    {
        if (map.IsOccupied(pos.X, pos.Y)) return;

        var biomeZone = map.GetBiome(pos);

        if (biomeZone == null || !biomesMap.TryGetValue(biomeZone.Value, out var biomeSettings)) return;

        if (rng.Randf() >= biomeSettings.PropDensityNormalized) return;

        var requiredTerrain = biomeZone.Value.IsWater() ? TileType.Water : TileType.Ground;

        var validRules = biomeSettings.PropRules;

        if (validRules.Count == 0) return;

        var rule = WeightedSelector.Pick(validRules, rule => rule.Weight, rng);
        var variation = db.GetRandomVariation(rule.Type, rng);

        if (variation != null) TryFitAndPlace(map, pos, variation, rule.Context, requiredTerrain);
    }

    private static bool TryFitAndPlace(
        CaMap map,
        Vector2I pos,
        PropData rule,
        PlacementContext context,
        TileType requiredTerrain
    )
    {
        var size = rule.ObjectSize;

        for (var dy = 0; dy >= -(size.Y - 1); dy--)
        for (var dx = 0; dx >= -(size.X - 1); dx--)
        {
            var offsetPos = new Vector2I(pos.X + dx, pos.Y + dy);

            if (!map.CanFitObject(offsetPos, size, requiredTerrain, true)) continue;

            if (!PropPlacementValidator.IsValidContextForArea(map, offsetPos, size, context)) continue;

            map.PlaceProp(offsetPos, rule.Tag, size);

            return true;
        }

        return false;
    }
}