using System.Linq;
using Godot;
using ProcGenLab.BSP.Models;
using ProcGenLab.Shared.Enums;
using ProcGenLab.Shared.Utils;

namespace ProcGenLab.BSP.Services;

public class CorridorBuilder
{
    private readonly CorridorEntranceResolver _entranceResolver = new();

    private readonly CorridorPathWriter _pathWriter = new();

    public int GetRoomConnectionCount(Room room)
    {
        return _entranceResolver.GetRoomConnectionCount(room);
    }

    public void Connect(BspNode root, BspMap map)
    {
        _entranceResolver.Reset();
        _pathWriter.Reset();
        ConnectNodes(root);
        map.PathsTiles = _pathWriter.PathTiles;
    }

    private void ConnectNodes(BspNode node)
    {
        if (node.IsLeaf || node.Left == null || node.Right == null)
            return;

        ConnectNodes(node.Left);
        ConnectNodes(node.Right);

        var (bestA, bestB) = FindBestRoomsToConnect(node.Left, node.Right);

        if (bestA == null || bestB == null)
            throw new LabException(this, "Failed to find rooms to connect");

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
            var score = GetConnectionScore(roomLeft, roomRight);

            if (score < lowestScore)
            {
                lowestScore = score;
                bestLeft = roomLeft;
                bestRight = roomRight;
            }
        }

        return (bestLeft, bestRight);
    }

    private float GetConnectionScore(Room roomA, Room roomB)
    {
        float distance = roomA.Rect.GetCenter().DistanceSquaredTo(roomB.Rect.GetCenter());
        var connections = GetRoomConnectionCount(roomA) + GetRoomConnectionCount(roomB);

        return distance + connections * 1500f;
    }

    private void BuildCorridorBetween(Room roomA, Room roomB, bool isHorizontalSplit)
    {
        var centerA = GridUtils.GetCenter(roomA.Rect);
        var centerB = GridUtils.GetCenter(roomB.Rect);

        var sideA = GetExitSide(centerA, centerB, isHorizontalSplit);
        var sideB = sideA.GetOpposite();

        if (isHorizontalSplit)
        {
            var start = _entranceResolver.Resolve(roomA, sideA, centerB.X);
            var end = _entranceResolver.Resolve(roomB, sideB, centerA.X);
            _pathWriter.BuildLPath(start, end);
        }
        else
        {
            var start = _entranceResolver.Resolve(roomA, sideA, centerB.Y);
            var end = _entranceResolver.Resolve(roomB, sideB, centerA.Y);
            _pathWriter.BuildLPath(start, end);
        }
    }

    private static Direction GetExitSide(Vector2I centerA, Vector2I centerB, bool isHorizontalSplit)
    {
        return isHorizontalSplit
            ? centerA.Y < centerB.Y ? Direction.South : Direction.North
            : centerA.X < centerB.X
                ? Direction.East
                : Direction.West;
    }
}
