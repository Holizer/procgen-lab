using System.Collections.Generic;
using System.Linq;
using Godot;
using ProcGenLab.Shared.Enums;
using ProcGenLab.Shared.Utils;
using ProcGenLab.Topology.Core;
using ProcGenLab.Topology.Models;
using ProcGenLab.WFC.Enums;
using ProcGenLab.WFC.Models;

namespace ProcGenLab.WFC.Services;

public static class TopologyPlacer
{
    public static void Apply(LevelTopology topology, WfcMap map)
    {
        map.CollapseMapToType(MacroTileType.Void);

        var nodes = topology.Nodes;
        if (nodes.Count == 0)
            return;

        var centerById = CalculateNodeCenters(nodes);

        var sortedEdges = BuildSortedConnectionList(nodes, centerById);

        var mstEdges = BuildMinimumSpanningTree(nodes, sortedEdges);

        var socketRequirements = TracePaths(mstEdges, centerById);

        var roomCenters = new HashSet<Vector2I>(centerById.Values);
        ApplyTiles(map, socketRequirements, roomCenters);
    }

    private static Dictionary<int, Vector2I> CalculateNodeCenters(IReadOnlyList<TopologyNode> nodes)
    {
        return nodes.ToDictionary(
            node => node.Id,
            node => GridUtils.GetCenter(node.Bounds)
        );
    }

    private static List<(int RoomA, int RoomB, float Distance)> BuildSortedConnectionList(
        IReadOnlyList<TopologyNode> nodes,
        Dictionary<int, Vector2I> roomCenters)
    {
        var connectionList = new List<(int RoomA, int RoomB, float Distance)>();
        var processedConnections = new HashSet<(int, int)>();

        foreach (var node in nodes)
        foreach (var connectedId in node.Connections)
        {
            var connectionKey = node.Id < connectedId
                ? (node.Id, connectedId)
                : (connectedId, node.Id);

            if (processedConnections.Add(connectionKey))
            {
                var distance = roomCenters[node.Id].DistanceTo(roomCenters[connectedId]);
                connectionList.Add((connectionKey.Item1, connectionKey.Item2, distance));
            }
        }

        connectionList.Sort((a, b) => a.Distance.CompareTo(b.Distance));
        return connectionList;
    }

    private static List<(int RoomA, int RoomB)> BuildMinimumSpanningTree(
        IReadOnlyList<TopologyNode> nodes,
        List<(int RoomA, int RoomB, float Distance)> sortedConnections)
    {
        var mstConnections = new List<(int RoomA, int RoomB)>();
        var disjointSet = new DisjointSet(nodes.Select(room => room.Id));

        foreach (var connection in sortedConnections)
            if (disjointSet.Union(connection.RoomA, connection.RoomB))
                mstConnections.Add((connection.RoomA, connection.RoomB));

        return mstConnections;
    }

    private static Dictionary<Vector2I, HashSet<Direction>> TracePaths(
        IReadOnlyList<(int RoomA, int RoomB)> mstConnections,
        IReadOnlyDictionary<int, Vector2I> roomCenters)
    {
        var socketRequirements = new Dictionary<Vector2I, HashSet<Direction>>();

        foreach (var (roomA, roomB) in mstConnections)
        {
            var startPos = roomCenters[roomA];
            var endPos = roomCenters[roomB];
            PathTracer.TraceBresenham(startPos, endPos, socketRequirements);
        }

        return socketRequirements;
    }

    private static void ApplyTiles(
        WfcMap map,
        IReadOnlyDictionary<Vector2I, HashSet<Direction>> socketRequirements,
        HashSet<Vector2I> roomCenters)
    {
        foreach (var (position, requiredDirections) in socketRequirements)
        {
            if (!GridUtils.InBounds(position.X, position.Y, map.Width, map.Height))
                continue;

            var hasNorth = requiredDirections.Contains(Direction.North);
            var hasEast = requiredDirections.Contains(Direction.East);
            var hasSouth = requiredDirections.Contains(Direction.South);
            var hasWest = requiredDirections.Contains(Direction.West);

            if (roomCenters.Contains(position))
            {
                map.SetCell(position.X, position.Y,
                    MacroTileResolver.ResolveRoom(hasNorth, hasEast, hasSouth, hasWest));
            }
            else
            {
                var possibleConnectors = MacroTileResolver.ResolveConnectors(hasNorth, hasEast, hasSouth, hasWest);
                if (possibleConnectors.Count > 0)
                    map[position.X, position.Y].RestorePossibleTypes(possibleConnectors);
            }
        }
    }
}