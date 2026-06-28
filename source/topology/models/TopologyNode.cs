using System.Collections.Generic;
using Godot;
using ProcGenLab.Topology.Enums;

namespace ProcGenLab.Topology.Models;

public class TopologyNode(int id, NodeRole role, Rect2I bounds)
{
    private readonly HashSet<int> _connections = new();

    public int Id { get; } = id;

    public NodeRole Role { get; set; } = role;

    public Rect2I Bounds { get; } = bounds;

    public IReadOnlySet<int> Connections => _connections;

    public bool AddConnection(int id)
    {
        return _connections.Add(id);
    }

    public bool RemoveConnection(int targetId)
    {
        return _connections.Remove(targetId);
    }
}