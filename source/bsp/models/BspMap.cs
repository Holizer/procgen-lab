using System.Collections.Generic;
using Godot;
using ProcGenLab.Shared.Core;
using ProcGenLab.Shared.Enums;

namespace ProcGenLab.BSP.Models;

public class BspMap(int width, int height) : Grid2D<TileType>(width, height)
{
    public IReadOnlyList<Room> Rooms { get; set; }

    public IReadOnlyList<Vector2I> PathsTiles { get; set; }

    public IEnumerable<Vector2I> GetFloorTiles()
    {
        foreach (var room in Rooms)
            for (var y = room.Rect.Position.Y; y < room.Rect.End.Y; y++)
            for (var x = room.Rect.Position.X; x < room.Rect.End.X; x++)
                yield return new Vector2I(x, y);

        foreach (var tile in PathsTiles)
            yield return tile;
    }
}