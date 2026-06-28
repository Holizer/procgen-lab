using System.Collections.Generic;
using ProcGenLab.Shared.Enums;
using ProcGenLab.WFC.Enums;
using ProcGenLab.WFC.Models;

namespace ProcGenLab.WFC.Services;

public static class WfcCellExtensions
{
    public static bool ConstrainByNeighbor(
        this WfcCell cell,
        HashSet<MacroTileType> neighborTypes,
        Direction dir,
        MacroRegistry registry
    )
    {
        var before = cell.Entropy;

        var allowed = new HashSet<MacroTileType>();
        foreach (var neighborType in neighborTypes)
            allowed.UnionWith(registry.GetCompatible(neighborType, dir));

        cell.PossibleTypes.IntersectWith(allowed);
        cell.SyncCollapsedState();

        return cell.Entropy < before;
    }
}