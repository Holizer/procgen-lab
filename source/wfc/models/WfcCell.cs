using System.Collections.Generic;
using ProcGenLab.WFC.Enums;

namespace ProcGenLab.WFC.Models;

public class WfcCell(IEnumerable<MacroTileType> allTypes)
{
    private MacroTileType? _collapsedType;

    public HashSet<MacroTileType> PossibleTypes { get; private set; } = [.. allTypes];

    public int Entropy => PossibleTypes.Count;

    public bool IsCollapsed => _collapsedType.HasValue;

    public MacroTileType? CollapsedType => _collapsedType;

    public void Collapse(MacroTileType type)
    {
        _collapsedType = type;
        PossibleTypes.Clear();
        PossibleTypes.Add(type);
    }

    public void RestorePossibleTypes(HashSet<MacroTileType> clone)
    {
        PossibleTypes = clone;
        SyncCollapsedState();
    }

    public void SyncCollapsedState()
    {
        if (PossibleTypes.Count != 1)
        {
            _collapsedType = null;

            return;
        }

        foreach (var type in PossibleTypes)
        {
            _collapsedType = type;

            return;
        }
    }

    public HashSet<MacroTileType> ClonePossibleTypes()
    {
        return [..PossibleTypes];
    }
}