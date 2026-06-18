using Godot;
using ProcGenLab.Shared.Enums;

namespace ProcGenLab.Shared.Utils;

public static partial class GridUtils
{
    public static readonly Vector2I InvalidTile = new(-1, -1);

    public static readonly Direction[] Directions =
    {
        Direction.North,
        Direction.South,
        Direction.East,
        Direction.West
    };

    public static readonly Vector2I[] DirectionOffsets =
    {
        new(0, -1),
        new(0, 1),
        new(-1, 0),
        new(1, 0)
    };
}