using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using ProcGenLab.Shared.Core;
using ProcGenLab.Shared.Enums;
using ProcGenLab.Shared.Utils;

namespace ProcGenLab.BSP.Models;

public class BspMap : Grid2D<TileType>
{
    public BspMap(int width, int height)
        : base(width, height)
    {
        Array.Fill(Grid, TileType.Wall);
    }

    public IReadOnlyList<Room> Rooms { get; set; }
    public IReadOnlyList<Vector2I> PathsTiles { get; set; }

    public ReadOnlySpan<TileType> AsSpan()
    {
        return Grid.AsSpan();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsType(int x, int y, TileType type)
    {
        return GridUtils.InBounds(x, y, Width, Height) && this[x, y] == type;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetTile(int x, int y, TileType type)
    {
        if (GridUtils.InBounds(x, y, Width, Height))
            this[x, y] = type;
    }
}