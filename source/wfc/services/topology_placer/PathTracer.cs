using System;
using System.Collections.Generic;
using Godot;
using ProcGenLab.Shared.Enums;

namespace ProcGenLab.WFC.Services;

public static class PathTracer
{
    public static void TraceBresenham(
        Vector2I from,
        Vector2I to,
        Dictionary<Vector2I, HashSet<Direction>> requirements
    )
    {
        int x = from.X,
            y = from.Y;

        var dx = Math.Abs(to.X - from.X);
        var dy = Math.Abs(to.Y - from.Y);
        var stepX = from.X < to.X ? 1 : -1;
        var stepY = from.Y < to.Y ? 1 : -1;
        var error = dx - dy;

        while (x != to.X || y != to.Y)
        {
            var e2 = error * 2;

            if (e2 > -dy)
            {
                var current = new Vector2I(x, y);
                error -= dy;
                x += stepX;
                var next = new Vector2I(x, y);
                var fwd = stepX > 0 ? Direction.East : Direction.West;
                AddRequirement(current, fwd, requirements);
                AddRequirement(next, fwd.GetOpposite(), requirements);
            }

            if (e2 < dx)
            {
                var current = new Vector2I(x, y);
                error += dx;
                y += stepY;
                var next = new Vector2I(x, y);
                var fwd = stepY > 0 ? Direction.South : Direction.North;
                AddRequirement(current, fwd, requirements);
                AddRequirement(next, fwd.GetOpposite(), requirements);
            }
        }
    }

    private static void AddRequirement(
        Vector2I pos,
        Direction dir,
        Dictionary<Vector2I, HashSet<Direction>> requirements
    )
    {
        if (!requirements.TryGetValue(pos, out var set))
            requirements[pos] = set = new HashSet<Direction>();

        set.Add(dir);
    }
}