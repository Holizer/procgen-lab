using System.Runtime.CompilerServices;
using Godot;

namespace ProcGenLab.Shared.Utils;

public static partial class GridUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool InBounds(int x, int y, int width, int height)
    {
        return (uint)x < (uint)width && (uint)y < (uint)height;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool InBounds(Vector2I coords, int width, int height)
    {
        return InBounds(coords.X, coords.Y, width, height);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool InBounds(Rect2I bounds, int x, int y)
    {
        return x >= bounds.Position.X && x < bounds.End.X && y >= bounds.Position.Y && y < bounds.End.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBorder(int x, int y, int width, int height, int borderWidth = 1)
    {
        return x < borderWidth || x >= width - borderWidth || y < borderWidth || y >= height - borderWidth;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBorder(Vector2I coords, int width, int height, int borderWidth = 1)
    {
        return IsBorder(coords.X, coords.Y, width, height, borderWidth);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2I GetCenter(Rect2I bounds)
    {
        return new Vector2I(bounds.Position.X + bounds.Size.X / 2, bounds.Position.Y + bounds.Size.Y / 2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetDistanceSquare(Vector2I a, Vector2I b)
    {
        return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
    }
}