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
}