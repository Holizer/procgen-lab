using System.Collections.Generic;
using ProcGenLab.Shared.Enums;
using ProcGenLab.WFC.Enums;

namespace ProcGenLab.WFC.Services;

public static class MacroTileResolver
{
    private static readonly Dictionary<
        (MacroSocketType N, MacroSocketType E, MacroSocketType S, MacroSocketType W),
        MacroTileType
    > RoomLookup;

    static MacroTileResolver()
    {
        RoomLookup =
            new Dictionary<(MacroSocketType N, MacroSocketType E, MacroSocketType S, MacroSocketType W),
                MacroTileType>();

        foreach (var type in MacroTileKindSets.RoomTypes)
            RoomLookup.TryAdd(SocketKey(type), type);
    }

    internal static MacroTileType ResolveRoom(bool n, bool e, bool s, bool w)
    {
        return RoomLookup.GetValueOrDefault(ToKey(n, e, s, w), MacroTileType.Void);
    }

    internal static HashSet<MacroTileType> ResolveConnectors(bool n, bool e, bool s, bool w)
    {
        var key = ToKey(n, e, s, w);
        var result = new HashSet<MacroTileType>();
        foreach (var t in MacroTileKindSets.ConnectorTypes)
            if (SocketKey(t) == key)
                result.Add(t);
        return result;
    }

    private static (MacroSocketType, MacroSocketType, MacroSocketType, MacroSocketType) SocketKey(
        MacroTileType type
    )
    {
        return (
            MacroTileSocketMap.Get(type, Direction.North),
            MacroTileSocketMap.Get(type, Direction.East),
            MacroTileSocketMap.Get(type, Direction.South),
            MacroTileSocketMap.Get(type, Direction.West)
        );
    }

    private static (MacroSocketType, MacroSocketType, MacroSocketType, MacroSocketType) ToKey(
        bool n,
        bool e,
        bool s,
        bool w
    )
    {
        return (
            n ? MacroSocketType.Corridor : MacroSocketType.Wall,
            e ? MacroSocketType.Corridor : MacroSocketType.Wall,
            s ? MacroSocketType.Corridor : MacroSocketType.Wall,
            w ? MacroSocketType.Corridor : MacroSocketType.Wall
        );
    }
}