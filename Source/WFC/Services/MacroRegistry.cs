using System.Collections.Generic;
using System.Linq;
using ProcGenLab.Shared.Enums;
using ProcGenLab.Shared.Utils;
using ProcGenLab.WFC.Enums;
using ProcGenLab.WFC.Resources;

namespace ProcGenLab.WFC.Services;

public class MacroRegistry
{
    private static readonly HashSet<MacroTileType> EmptyCompatible = [];

    private readonly Dictionary<
        MacroTileType,
        Dictionary<Direction, HashSet<MacroTileType>>
    > _compatible = new();

    private readonly Dictionary<MacroTileType, float> _weights = new();

    public MacroRegistry(WfcChunkCatalog catalog, WfcWeightConfig weights)
    {
        if (catalog?.Tiles == null)
        {
            this.LogWarning("Catalog is null");
            return;
        }

        foreach (var tile in catalog.Tiles)
            if (tile != null)
                Tiles[tile.Type] = tile;

        if (weights?.Weights != null)
            foreach (var entry in weights.Weights)
                _weights[entry.Type] = entry.Weight;

        BuildCompatibilityTable();
    }

    public Dictionary<MacroTileType, MacroTile> Tiles { get; } = new();

    public float GetWeight(MacroTileType type)
    {
        return _weights.GetValueOrDefault(type, 1.0f);
    }

    private void BuildCompatibilityTable()
    {
        var types = Tiles.Keys.ToList();
        foreach (var t in types)
        {
            _compatible[t] = new Dictionary<Direction, HashSet<MacroTileType>>();
            foreach (var dir in GridUtils.Directions)
                _compatible[t][dir] = new HashSet<MacroTileType>();
        }

        foreach (var t1 in types)
        foreach (var t2 in types)
        foreach (var dir in GridUtils.Directions)
            if (Tiles[t1].GetSocket(dir) == Tiles[t2].GetSocket(dir.GetOpposite()))
                _compatible[t1][dir].Add(t2);
    }

    public HashSet<MacroTileType> GetCompatible(MacroTileType type, Direction dir)
    {
        return _compatible.TryGetValue(type, out var dirs) && dirs.TryGetValue(dir, out var set)
            ? set
            : [];
    }

    public bool AreCompatible(MacroTileType t1, MacroTileType t2, Direction dir)
    {
        return _compatible.TryGetValue(t1, out var dirs)
               && dirs.TryGetValue(dir, out var set)
               && set.Contains(t2);
    }
}