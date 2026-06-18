using System.Collections.Generic;
using System.Linq;
using Godot;
using ProcGenLab.BSP.Models;
using ProcGenLab.Shared.Enums;
using ProcGenLab.Shared.Utils;

namespace ProcGenLab.BSP.Services;

public class CorridorBuilder
{
    private readonly Dictionary<(Room Room, Direction Dir), Vector2I> _entrances = new();
    private readonly List<Vector2I> _pathTiles = new();

    public int GetRoomConnectionCount(Room room)
    {
        if (room == null)
            return 0;
        return _entrances.Keys.Count(key => key.Room == room);
    }

    public void Connect(BspNode root, BspMap map)
    {
        _pathTiles.Clear();
        _entrances.Clear();

        ConnectNodes(root);

        map.PathsTiles = _pathTiles;
    }

    private void ConnectNodes(BspNode node)
    {
        if (node.IsLeaf || node.Left == null || node.Right == null)
            return;

        ConnectNodes(node.Left);
        ConnectNodes(node.Right);

        var roomsLeft = new List<Room>();
        var roomsRight = new List<Room>();

        GetRoomsInSubtree(node.Left, roomsLeft);
        GetRoomsInSubtree(node.Right, roomsRight);

        if (roomsLeft.Count == 0 || roomsRight.Count == 0)
            return;

        Room bestA = null;
        Room bestB = null;
        var minScore = float.MaxValue;

        foreach (var rA in roomsLeft)
        foreach (var rB in roomsRight)
        {
            float distSq = rA.Rect.GetCenter().DistanceSquaredTo(rB.Rect.GetCenter());

            var existingConnections = GetRoomConnectionCount(rA) + GetRoomConnectionCount(rB);

            var score = distSq + existingConnections * 1500f;
            if (score < minScore)
            {
                minScore = score;
                bestA = rA;
                bestB = rB;
            }
        }

        if (bestA == null || bestB == null)
            return;

        var centerA = bestA.Rect.GetCenter();
        var centerB = bestB.Rect.GetCenter();

        Direction sideA;
        if (node.IsHorizontal)
            sideA = centerB.Y > centerA.Y ? Direction.South : Direction.North;
        else
            sideA = centerB.X > centerA.X ? Direction.East : Direction.West;

        var sideB = sideA.GetOpposite();

        var start = GridUtils.GetCenter(bestA.Rect);
        var end = GridUtils.GetCenter(bestB.Rect);

        _entrances[(bestA, sideA)] = start;
        _entrances[(bestB, sideB)] = end;

        CreateLPath(start, end);
    }

    private void GetRoomsInSubtree(BspNode node, List<Room> rooms)
    {
        if (node == null)
            return;
        if (node.IsLeaf && node.Room != null)
        {
            rooms.Add(node.Room);
            return;
        }

        GetRoomsInSubtree(node.Left!, rooms);
        GetRoomsInSubtree(node.Right!, rooms);
    }

    private void CreateLPath(Vector2I start, Vector2I end)
    {
        var current = start;
        _pathTiles.Add(current);
        MoveX(ref current, end.X);
        MoveY(ref current, end.Y);
    }

    private void MoveX(ref Vector2I current, int targetX)
    {
        var stepX = targetX > current.X ? 1 : -1;
        while (current.X != targetX)
        {
            _pathTiles.Add(current);
            current.X += stepX;
        }
    }

    private void MoveY(ref Vector2I current, int targetY)
    {
        var stepY = targetY > current.Y ? 1 : -1;
        while (current.Y != targetY)
        {
            _pathTiles.Add(current);
            current.Y += stepY;
        }
    }
}