using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ProcGenLab.Shared.Core;
using ProcGenLab.Shared.Utils;
using ProcGenLab.Topology.Core;
using ProcGenLab.WFC.Enums;

namespace ProcGenLab.WFC.Models;

public class WfcMap : Grid2D<WfcCell>
{
    public WfcMap(int width, int height, IEnumerable<MacroTileType> allTypes)
        : base(width, height)
    {
        var cachedTypes = allTypes.ToArray();
        for (var i = 0; i < Grid.Length; i++)
            Grid[i] = new WfcCell(cachedTypes);
    }

    public LevelTopology Topology { get; set; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetCell(int x, int y, MacroTileType type)
    {
        if (GridUtils.InBounds(x, y, Width, Height))
            Grid[GetIndex(x, y)].Collapse(type);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Ban(int x, int y, MacroTileType type)
    {
        if (GridUtils.InBounds(x, y, Width, Height))
            Grid[GetIndex(x, y)].PossibleTypes.Remove(type);
    }

    public void CollapseMapToType(MacroTileType type)
    {
        foreach (var cell in Grid)
            cell.Collapse(type);
    }
}