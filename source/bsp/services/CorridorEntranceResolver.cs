using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using ProcGenLab.BSP.Models;
using ProcGenLab.Shared.Enums;

namespace ProcGenLab.BSP.Services;

public class CorridorEntranceResolver
{
    private const int WallInset = 1;

    private readonly Dictionary<(Room Room, Direction Dir), List<Vector2I>> _entrances = new();

    public int GetRoomConnectionCount(Room room)
    {
        return room == null ? 0 : _entrances.Keys.Count(key => key.Room == room);
    }

    public void Reset()
    {
        _entrances.Clear();
    }

    public Vector2I Resolve(Room room, Direction side, int preferred, int minGap = 2)
    {
        var (min, max) = GetAxisBounds(room, side);

        if (!_entrances.TryGetValue((room, side), out var entrancesList))
            _entrances[(room, side)] = entrancesList = [];

        var candidate = FindFreeCandidate(entrancesList, side, Math.Clamp(preferred, min, max), min, max, minGap);
        var point = side.GetWallPoint(room.Rect, candidate);

        entrancesList.Add(point);

        return point;
    }

    private static (int Min, int Max) GetAxisBounds(Room room, Direction side)
    {
        var rect = room.Rect;

        return side is Direction.North or Direction.South
            ? (rect.Position.X + WallInset, rect.End.X - WallInset - 1)
            : (rect.Position.Y + WallInset, rect.End.Y - WallInset - 1);
    }

    private static int FindFreeCandidate(List<Vector2I> entrances, Direction side, int candidate, int min, int max,
        int minGap)
    {
        foreach (var entrance in entrances)
        {
            var coord = side is Direction.North or Direction.South ? entrance.X : entrance.Y;
            if (Math.Abs(coord - candidate) < minGap)
                candidate = Math.Clamp(candidate + minGap, min, max);
        }

        return candidate;
    }
}
