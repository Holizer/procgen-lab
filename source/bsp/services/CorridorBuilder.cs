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

    private readonly List<Vector2I> _pathTiles = [];

    public int GetRoomConnectionCount(Room room)
    {
        return room == null ? 0 : _entrances.Keys.Count(key => key.Room == room);
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

        var (bestA, bestB) = FindBestRoomsToConnect(node.Left, node.Right);

        if (bestA == null || bestB == null)
            return;

        BuildCorridorBetween(bestA, bestB, node.IsHorizontal);
    }

    private (Room, Room) FindBestRoomsToConnect(BspNode leftNode, BspNode rightNode)
    {
        var roomsLeft = leftNode.GetRooms().ToArray();
        var roomsRight = rightNode.GetRooms().ToArray();

        Room bestLeft = null;
        Room bestRight = null;

        var lowestScore = float.MaxValue;

        foreach (var roomLeft in roomsLeft)
        foreach (var roomRight in roomsRight)
        {
            var currentScore = GetConnectionScore(roomLeft, roomRight);

            if (currentScore < lowestScore)
            {
                lowestScore = currentScore;
                bestLeft = roomLeft;
                bestRight = roomRight;
            }
        }

        return (bestLeft, bestRight);
    }

    private float GetConnectionScore(Room roomA, Room roomB)
    {
        float distance = roomA.Rect.GetCenter().DistanceSquaredTo(roomB.Rect.GetCenter());
        var totalConnections = GetRoomConnectionCount(roomA) + GetRoomConnectionCount(roomB);

        return distance + totalConnections * 1500f;
    }

    private void BuildCorridorBetween(Room roomA, Room roomB, bool isHorizontalSplit)
    {
        var centerA = roomA.Rect.GetCenter();
        var centerB = roomB.Rect.GetCenter();

        Direction sideA;
        if (isHorizontalSplit)
            sideA = centerB.Y > centerA.Y ? Direction.South : Direction.North;
        else
            sideA = centerB.X > centerA.X ? Direction.East : Direction.West;

        var sideB = sideA.GetOpposite();

        var start = GridUtils.GetCenter(roomA.Rect);
        var end = GridUtils.GetCenter(roomB.Rect);

        _entrances[(roomA, sideA)] = start;
        _entrances[(roomB, sideB)] = end;

        CreateLPath(start, end);
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
            current.X += stepX;
            _pathTiles.Add(current);
        }
    }

    private void MoveY(ref Vector2I current, int targetY)
    {
        var stepY = targetY > current.Y ? 1 : -1;

        while (current.Y != targetY)
        {
            current.Y += stepY;
            _pathTiles.Add(current);
        }
    }
}