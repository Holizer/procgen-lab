using System.Collections.Generic;
using Godot;

namespace ProcGenLab.BSP.Services;

public class CorridorPathWriter
{
    private readonly List<Vector2I> _pathTiles = [];

    public IReadOnlyList<Vector2I> PathTiles => _pathTiles;

    public void Reset()
    {
        _pathTiles.Clear();
    }

    public void BuildLPath(Vector2I start, Vector2I end)
    {
        var current = start;
        _pathTiles.Add(current);
        MoveX(ref current, end.X);
        MoveY(ref current, end.Y);
    }

    private void MoveX(ref Vector2I current, int targetX)
    {
        var step = targetX > current.X ? 1 : -1;

        while (current.X != targetX)
        {
            current.X += step;
            _pathTiles.Add(current);
        }
    }

    private void MoveY(ref Vector2I current, int targetY)
    {
        var step = targetY > current.Y ? 1 : -1;

        while (current.Y != targetY)
        {
            current.Y += step;
            _pathTiles.Add(current);
        }
    }
}
