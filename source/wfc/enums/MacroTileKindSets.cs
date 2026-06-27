using System.Collections.Frozen;

namespace ProcGenLab.WFC.Enums;

public static class MacroTileKindSets
{
    public static readonly FrozenSet<MacroTileType> RoomTypes = new[]
    {
        MacroTileType.RoomN,
        MacroTileType.RoomE,
        MacroTileType.RoomS,
        MacroTileType.RoomW,
        MacroTileType.RoomNE,
        MacroTileType.RoomNS,
        MacroTileType.RoomNW,
        MacroTileType.RoomSE,
        MacroTileType.RoomSW,
        MacroTileType.RoomEW,
        MacroTileType.RoomNES,
        MacroTileType.RoomNEW,
        MacroTileType.RoomNSW,
        MacroTileType.RoomSEW,
        MacroTileType.RoomNESW
    }.ToFrozenSet();

    public static readonly FrozenSet<MacroTileType> ConnectorTypes = new[]
    {
        MacroTileType.CorridorH,
        MacroTileType.CorridorV,
        MacroTileType.CornerNE,
        MacroTileType.CornerNW,
        MacroTileType.CornerSE,
        MacroTileType.CornerSW,
        MacroTileType.TJunctionN,
        MacroTileType.TJunctionE,
        MacroTileType.TJunctionS,
        MacroTileType.TJunctionW,
        MacroTileType.Cross
    }.ToFrozenSet();
}