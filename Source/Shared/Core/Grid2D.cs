using System.Runtime.CompilerServices;
using Godot;

namespace ProcGenLab.Shared.Core;

public abstract class Grid2D(int width, int height)
{
    public int Width { get; } = width;
    public int Height { get; } = height;
    public int TotalCells => Width * Height;
}

public abstract class Grid2D<TCell> : Grid2D
{
    protected Grid2D(int width, int height)
        : base(width, height)
    {
        Grid = new TCell[TotalCells];
    }

    public TCell[] Grid { get; }

    public ref TCell this[int x, int y]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref Grid[GetIndex(x, y)];
    }

    public ref TCell this[in Vector2I coords]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref Grid[GetIndex(coords)];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetIndex(int x, int y)
    {
        return x + y * Width;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetIndex(in Vector2I coords)
    {
        return GetIndex(coords.X, coords.Y);
    }
}