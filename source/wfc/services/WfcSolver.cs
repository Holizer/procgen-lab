using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using ProcGenLab.Shared.Enums;
using ProcGenLab.Shared.Utils;
using ProcGenLab.WFC.Enums;
using ProcGenLab.WFC.Models;

namespace ProcGenLab.WFC.Services;

public class WfcSolver(WfcMap map, MacroRegistry registry, RandomNumberGenerator rng)
{
    private readonly HashSet<MacroTileType> _allowedScratch = [];

    private readonly Stack<Vector2I> _propStack = new(256);

    public bool Solve(int maxAttempts = 256)
    {
        var snapshot = SnapshotDomains();

        for (var attempt = 0; attempt < maxAttempts; attempt++)
        {
            if (attempt > 0)
                RestoreDomains(snapshot);

            if (RunCollapsePass())
                return true;
        }

        return false;
    }

    private HashSet<MacroTileType>[] SnapshotDomains()
    {
        var snap = new HashSet<MacroTileType>[map.TotalCells];
        for (var i = 0; i < map.TotalCells; i++)
            snap[i] = map.Grid[i].ClonePossibleTypes();

        return snap;
    }

    private void RestoreDomains(HashSet<MacroTileType>[] snap)
    {
        for (var i = 0; i < map.TotalCells; i++)
            map.Grid[i].RestorePossibleTypes(snap[i]);
    }

    private bool RunCollapsePass()
    {
        while (true)
        {
            var coords = PickLowestEntropy();

            if (coords == null)
                return true;

            var cell = map[coords.Value];

            if (cell.Entropy == 0)
                return false;

            cell.Collapse(PickByWeight(cell));
            _propStack.Push(coords.Value);

            if (!Propagate())
                return false;
        }
    }

    private bool Propagate()
    {
        while (_propStack.Count > 0)
        {
            var pos = _propStack.Pop();
            var current = map[pos];

            foreach (var dir in GridUtils.Directions)
            {
                var offset = dir.GetVector();
                var nx = pos.X + offset.X;
                var ny = pos.Y + offset.Y;

                if (!GridUtils.InBounds(nx, ny, map.Width, map.Height))
                    continue;

                var neighbor = map[nx, ny];

                if (neighbor.IsCollapsed)
                    continue;

                if (!TryConstrainByNeighbor(neighbor, current.PossibleTypes, dir))
                    continue;

                if (neighbor.Entropy == 0)
                    return false;

                _propStack.Push(new Vector2I(nx, ny));
            }
        }

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector2I? PickLowestEntropy()
    {
        Vector2I? best = null;
        var min = int.MaxValue;

        for (var i = 0; i < map.TotalCells; i++)
        {
            var cell = map.Grid[i];

            if (cell.IsCollapsed || cell.Entropy >= min)
                continue;

            min = cell.Entropy;
            best = new Vector2I(i % map.Width, i / map.Width);

            if (min == 1)
                break;
        }

        return best;
    }

    private MacroTileType PickByWeight(WfcCell cell)
    {
        var possibleTypes = cell.PossibleTypes;
        var total = 0f;

        foreach (var type in possibleTypes)
            total += registry.GetWeight(type);

        var roll = rng.RandfRange(0f, total);
        var cursor = 0f;

        MacroTileType last = default;

        foreach (var type in possibleTypes)
        {
            last = type;
            cursor += registry.GetWeight(type);

            if (roll <= cursor)
                return type;
        }

        return last;
    }

    private bool TryConstrainByNeighbor(
        WfcCell cell,
        HashSet<MacroTileType> neighborTypes,
        Direction dir
    )
    {
        var before = cell.Entropy;
        _allowedScratch.Clear();

        foreach (var neighborType in neighborTypes)
            _allowedScratch.UnionWith(registry.GetCompatible(neighborType, dir));

        cell.PossibleTypes.IntersectWith(_allowedScratch);
        cell.SyncCollapsedState();

        return cell.Entropy < before;
    }
}