using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using ProcGenLab.CellularAutomata.Enums;
using ProcGenLab.CellularAutomata.Models;
using ProcGenLab.CellularAutomata.Resources;
using ProcGenLab.Shared.Enums;
using ProcGenLab.Shared.Utils;

namespace ProcGenLab.CellularAutomata.Services;

public class BiomeCreator
{
    private readonly List<BiomeSettings> _landBiomes;
    private readonly int _minRegionSizeForSplitting;

    public BiomeCreator(IEnumerable<BiomeSettings> biomes, int minRegionSizeForSplitting = 150)
    {
        _landBiomes = biomes?.Where(biome => biome.Type.IsLand()).ToList() ?? [];
        _minRegionSizeForSplitting = minRegionSizeForSplitting;

        if (_landBiomes.Count == 0) this.LogWarning("There are no land biomes configured.");
    }

    public void AssignBiomes(
        CaMap map,
        List<List<Vector2I>> regions,
        RandomNumberGenerator rng,
        float frequency
    )
    {
        if (_landBiomes.Count == 0 || regions == null)
            return;

        var noise = new FastNoiseLite
        {
            Seed = (int)rng.Randi(),
            Frequency = frequency,
            NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin
        };

        foreach (var region in regions)
        {
            if (region == null || region.Count == 0)
                continue;

            if (map.IsRegionType(region, TileType.Water))
                AssignWaterRegion(map, region);
            else if (region.Count < _minRegionSizeForSplitting)
                AssignSingleBiomeByCentroid(map, region, noise);
            else
                AssignLandRegionWithNoise(map, region, noise);
        }
    }

    private void AssignWaterRegion(CaMap map, List<Vector2I> region)
    {
        var isLake = !region.Any(pos => GridUtils.IsBorder(pos, map.Width, map.Height));
        var waterBiome = isLake ? BiomeZone.Lake : BiomeZone.Sea;

        foreach (var pos in region) map.AssignBiome(pos, waterBiome);
    }

    private void AssignSingleBiomeByCentroid(CaMap map, List<Vector2I> region, FastNoiseLite noise)
    {
        var center = GridUtils.GetCentroid(region);
        var noiseValue = noise.GetNoise2D(center.X, center.Y);
        var biomeType = GetBiomeByNoise(noiseValue);

        foreach (var pos in region) map.AssignBiome(pos, biomeType);
    }

    private void AssignLandRegionWithNoise(CaMap map, List<Vector2I> region, FastNoiseLite noise)
    {
        foreach (var pos in region)
        {
            var noiseValue = noise.GetNoise2D(pos.X, pos.Y);
            var biomeType = GetBiomeByNoise(noiseValue);
            map.AssignBiome(pos, biomeType);
        }
    }

    private BiomeZone GetBiomeByNoise(float noiseValue)
    {
        const float expectedMinNoise = -0.2f;
        const float expectedMaxNoise = 0.2f;

        var normalized = Mathf.InverseLerp(expectedMinNoise, expectedMaxNoise, noiseValue);

        var index = Math.Clamp(
            (int)(normalized * _landBiomes.Count),
            0,
            _landBiomes.Count - 1
        );

        return _landBiomes[index].Type;
    }
}