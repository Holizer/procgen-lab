using System.Collections.Generic;
using Godot;
using ProcGenLab.Shared.Utils;
using ProcGenLab.Topology.Enums;
using ProcGenLab.Topology.Models;

namespace ProcGenLab.Topology.Core;

public class LevelTopology
{
    private readonly List<TopologyNode> _nodes = new();
    public IReadOnlyList<TopologyNode> Nodes => _nodes;

    public TopologyNode AddNode(NodeRole role, Rect2I bounds)
    {
        var node = new TopologyNode(_nodes.Count, role, bounds);
        _nodes.Add(node);
        return node;
    }

    public bool Connect(int aId, int bId)
    {
        if ((uint)aId >= (uint)_nodes.Count || (uint)bId >= (uint)_nodes.Count)
        {
            this.LogError($"Out of bounds {aId}<->{bId}");
            return false;
        }

        var a = _nodes[aId].AddConnection(bId);
        var b = _nodes[bId].AddConnection(aId);
        return a && b;
    }
}