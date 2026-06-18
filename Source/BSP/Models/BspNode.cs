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
}