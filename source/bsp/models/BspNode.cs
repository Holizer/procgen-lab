using System.Collections.Generic;
using Godot;

namespace ProcGenLab.BSP.Models;

public class BspNode(Rect2I area, int depth = 0)
{
    public Rect2I Area { get; } = area;
    public int Depth { get; set; } = depth;
    public BspNode Left { get; set; }
    public BspNode Right { get; set; }
    public bool IsHorizontal { get; set; }
    public Room Room { get; set; }

    public bool IsLeaf => Left == null && Right == null;

    public IEnumerable<Room> GetRooms()
    {
        if (IsLeaf)
        {
            if (Room != null) yield return Room;
            yield break;
        }

        if (Left != null)
            foreach (var room in Left.GetRooms())
                yield return room;

        if (Right != null)
            foreach (var room in Right.GetRooms())
                yield return room;
    }
}