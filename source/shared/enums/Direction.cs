using System;
using Godot;

namespace ProcGenLab.Shared.Enums;

public enum Direction
{
    North = 0,

    East = 1,

    South = 2,

    West = 3
}

public static class DirectionExtensions
{
    public static Direction GetOpposite(this Direction direction)
    {
        return direction switch
        {
            Direction.North => Direction.South,
            Direction.South => Direction.North,
            Direction.East => Direction.West,
            Direction.West => Direction.East,
            _ => direction
        };
    }

    public static Vector2I GetVector(this Direction direction)
    {
        return direction switch
        {
            Direction.North => new Vector2I(0, -1),
            Direction.South => new Vector2I(0, 1),
            Direction.East => new Vector2I(1, 0),
            Direction.West => new Vector2I(-1, 0),
            _ => Vector2I.Zero
        };
    }

    public static Vector2I GetWallPoint(this Direction side, Rect2I rect, int candidate)
    {
        return side switch
        {
            Direction.North => new Vector2I(candidate, rect.Position.Y),
            Direction.South => new Vector2I(candidate, rect.End.Y - 1),
            Direction.East => new Vector2I(rect.End.X - 1, candidate),
            Direction.West => new Vector2I(rect.Position.X, candidate),
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };
    }
}
