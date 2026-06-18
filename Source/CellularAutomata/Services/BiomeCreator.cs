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

    private readonly bool IsFailed;

    public BiomeCreator(IEnumerable<BiomeSettings> biomes)
    {
        _landBiomes = biomes?.Where(b => b.Type.IsLand()).ToList() ?? new List<BiomeSettings>();

        if (_landBiomes.Count == 0)
        {
            this.LogWarning("There are no land biomes here.");
            IsFailed = true;
        }
    }

    public void AssignBiomes(
        CaMap map,
        List<List<Vector2I>> regions,
        RandomNumberGenerator rng,
        float frequency
    )
    {
        if (IsFailed)
            return;

        var noise = new FastNoiseLite
        {
            Seed = (int)rng.Randi(),
            Frequency = frequency,
            NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin
        };

        foreach (var region in regions)
        {
            if (region.Count == 0)
                continue;

            var isWaterRegion = map.IsType(region[0], TileType.Water);
            if (isWaterRegion)
            {
                var isLake = !region.Any(pos => GridUtils.IsBorder(pos, map.Width, map.Height));
                var waterBiome = isLake ? BiomeZone.Lake : BiomeZone.Sea;
                foreach (var pos in region)
                    map.AssignBiome(pos, waterBiome);
                continue;
            }

            if (region.Count < _landBiomes.Count * 10)
            {
                var center = GridUtils.GetCentroid(region);
                var centerNoise = noise.GetNoise2D(center.X, center.Y);
                var index = Math.Clamp(
                    (int)((centerNoise + 1f) / 2f * _landBiomes.Count),
                    0,
                    _landBiomes.Count - 1
                );

                foreach (var pos in region)
                    map.AssignBiome(pos, _landBiomes[index].Type);

                continue;
            }

            var noiseValues = region.Select(pos => noise.GetNoise2D(pos.X, pos.Y)).ToList();
            var min = noiseValues.Min();
            var max = noiseValues.Max();
            var range = max - min;

            for (var i = 0; i < region.Count; i++)
            {
                var normalized = range > 0.001f ? (noiseValues[i] - min) / range : 0.5f;

                var index = Math.Clamp(
                    (int)(normalized * _landBiomes.Count),
                    0,
                    _landBiomes.Count - 1
                );

                map.AssignBiome(region[i], _landBiomes[index].Type);
            }
        }
    }
}