using System.Collections.Generic;
using Godot;
using ProcGenLab.BSP.Models;
using ProcGenLab.Topology.Core;
using ProcGenLab.Topology.Enums;
using ProcGenLab.Topology.Models;

namespace ProcGenLab.WFC.Services;

public static class BspToTopologyConverter
{
    public static LevelTopology Convert(BspNode root)
    {
        var topology = new LevelTopology();
        var leaves = new List<BspNode>();
        CollectLeaves(root, leaves);

        var nodeMap = new Dictionary<BspNode, TopologyNode>(leaves.Count);
        foreach (var leaf in leaves)
            nodeMap[leaf] = topology.AddNode(NodeRole.Branch, leaf.Area);

        for (var i = 0; i < leaves.Count; i++)
        for (var j = i + 1; j < leaves.Count; j++)
            if (AreAdjacent(leaves[i].Area, leaves[j].Area))
                topology.Connect(nodeMap[leaves[i]].Id, nodeMap[leaves[j]].Id);

        return topology;
    }

    private static void CollectLeaves(BspNode node, List<BspNode> result)
    {
        if (node.IsLeaf)
        {
            result.Add(node);

            return;
        }

        CollectLeaves(node.Left, result);
        CollectLeaves(node.Right, result);
    }

    private static bool AreAdjacent(Rect2I a, Rect2I b)
    {
        var xOverlap = a.Position.X < b.End.X && b.Position.X < a.End.X;
        var yOverlap = a.Position.Y < b.End.Y && b.Position.Y < a.End.Y;
        var xTouches = a.End.X == b.Position.X || b.End.X == a.Position.X;
        var yTouches = a.End.Y == b.Position.Y || b.End.Y == a.Position.Y;

        return (xTouches && yOverlap) || (yTouches && xOverlap);
    }
}