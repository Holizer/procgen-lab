using System.Collections.Frozen;
using System.Collections.Generic;
using ProcGenLab.Shared.Enums;
using ProcGenLab.WFC.Enums;

namespace ProcGenLab.WFC.Services;

public static class MacroTileSocketMap
{
    private const MacroSocketType W = MacroSocketType.Wall;

    private const MacroSocketType C = MacroSocketType.Corridor;

    private static readonly FrozenDictionary<
        MacroTileType,
        (MacroSocketType N, MacroSocketType E, MacroSocketType S, MacroSocketType W)
    > SocketMap = new Dictionary<
        MacroTileType,
        (MacroSocketType, MacroSocketType, MacroSocketType, MacroSocketType)
    >
    {
        [MacroTileType.Void] = (W, W, W, W),
        [MacroTileType.CorridorH] = (W, C, W, C),
        [MacroTileType.CorridorV] = (C, W, C, W),
        [MacroTileType.CornerNE] = (C, C, W, W),
        [MacroTileType.CornerNW] = (C, W, W, C),
        [MacroTileType.CornerSE] = (W, C, C, W),
        [MacroTileType.CornerSW] = (W, W, C, C),
        [MacroTileType.TJunctionN] = (C, C, W, C),
        [MacroTileType.TJunctionE] = (C, C, C, W),
        [MacroTileType.TJunctionS] = (W, C, C, C),
        [MacroTileType.TJunctionW] = (C, W, C, C),
        [MacroTileType.Cross] = (C, C, C, C),

        [MacroTileType.RoomN] = (C, W, W, W),
        [MacroTileType.RoomE] = (W, C, W, W),
        [MacroTileType.RoomS] = (W, W, C, W),
        [MacroTileType.RoomW] = (W, W, W, C),

        [MacroTileType.RoomNE] = (C, C, W, W),
        [MacroTileType.RoomNS] = (C, W, C, W),
        [MacroTileType.RoomNW] = (C, W, W, C),
        [MacroTileType.RoomSE] = (W, C, C, W),
        [MacroTileType.RoomEW] = (W, C, W, C),
        [MacroTileType.RoomSW] = (W, W, C, C),

        [MacroTileType.RoomNES] = (C, C, C, W),
        [MacroTileType.RoomNEW] = (C, C, W, C),
        [MacroTileType.RoomNSW] = (C, W, C, C),
        [MacroTileType.RoomSEW] = (W, C, C, C),

        [MacroTileType.RoomNESW] = (C, C, C, C)
    }.ToFrozenDictionary();

    public static MacroSocketType Get(MacroTileType type, Direction dir)
    {
        return SocketMap.TryGetValue(type, out var socket)
            ? dir switch
            {
                Direction.North => socket.N,
                Direction.East => socket.E,
                Direction.South => socket.S,
                Direction.West => socket.W,
                _ => W
            }
            : W;
    }
}