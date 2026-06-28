using System.Collections.Generic;
using System.Linq;
using Godot;
using ProcGenLab.BSP.Enums;
using ProcGenLab.BSP.Models;

namespace ProcGenLab.BSP.Services;

public partial class RoomDecorator
{
    public void PlaceObjects(IReadOnlyList<Room> rooms)
    {
        PlaceKey(rooms);
        foreach (var room in rooms)
            PlaceRoomObjects(room);
    }

    private void PlaceKey(IReadOnlyList<Room> rooms)
    {
        var standardRooms = rooms.Where(r => r.Type == RoomType.Standard).ToList();

        if (standardRooms.Count == 0)
            return;

        var keyRoom = standardRooms[rng.RandiRange(0, standardRooms.Count - 1)];
        keyRoom.AddObject(new GameObject(ObjectType.Key, GetRandomPos(keyRoom)));
    }

    private void PlaceRoomObjects(Room room)
    {
        switch (room.Type)
        {
            case RoomType.Treasure:
                PlaceTreasureObjects(room);

                break;
            case RoomType.Boss:
                PlaceBossObjects(room);

                break;
            case RoomType.Standard:
                PlaceStandardObjects(room);

                break;
        }
    }

    private void PlaceTreasureObjects(Room room)
    {
        var floor = room.Rect.Grow(-1);
        room.AddObject(new GameObject(ObjectType.Chest, room.Rect.GetCenter()));
        room.AddObject(new GameObject(ObjectType.Coin, floor.Position));
        room.AddObject(
            new GameObject(ObjectType.Coin, new Vector2I(floor.End.X - 1, floor.Position.Y))
        );

        room.AddObject(
            new GameObject(ObjectType.Coin, new Vector2I(floor.Position.X, floor.End.Y - 1))
        );

        room.AddObject(new GameObject(ObjectType.Coin, floor.End - Vector2I.One));
    }

    private void PlaceBossObjects(Room room)
    {
        room.AddObject(new GameObject(ObjectType.ExitGate, room.Rect.GetCenter()));
    }

    private void PlaceStandardObjects(Room room)
    {
        var count = rng.RandiRange(1, 2);
        for (var i = 0; i < count; i++)
            room.AddObject(new GameObject(ObjectType.Flask, GetRandomPos(room)));
    }
}