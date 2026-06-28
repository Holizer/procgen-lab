using System.Collections.Generic;
using Godot;
using ProcGenLab.BSP.Enums;
using ProcGenLab.BSP.Models;
using ProcGenLab.BSP.Resources;

namespace ProcGenLab.BSP.Services;

public partial class RoomDecorator(RandomNumberGenerator rng)
{
    public void Decorate(IReadOnlyList<Room> rooms, CorridorBuilder builder, BspConfig config)
    {
        AssignTypes(rooms, builder, config.MaxTreasureRooms);
        PlaceObjects(rooms);
        PlaceEnemies(rooms, config.EnemyConfig);
    }

    public void AssignTypes(
        IReadOnlyList<Room> rooms,
        CorridorBuilder builder,
        int maxTreasureRoomsCounts = 1
    )
    {
        if (rooms.Count < 2)
            return;

        var shuffledRooms = new List<Room>(rooms);

        for (var i = shuffledRooms.Count - 1; i > 0; i--)
        {
            var j = rng.RandiRange(0, i);
            (shuffledRooms[i], shuffledRooms[j]) = (shuffledRooms[j], shuffledRooms[i]);
        }

        var spawnRoom = shuffledRooms[0];
        spawnRoom.Type = RoomType.Spawn;

        var deadEnds = new List<Room>();
        for (var i = 1; i < shuffledRooms.Count; i++)
            if (builder.GetRoomConnectionCount(shuffledRooms[i]) == 1)
                deadEnds.Add(shuffledRooms[i]);

        if (deadEnds.Count > 0)
        {
            deadEnds.Sort((a, b) =>
                a
                    .Rect.GetCenter()
                    .DistanceSquaredTo(spawnRoom.Rect.GetCenter())
                    .CompareTo(b.Rect.GetCenter().DistanceSquaredTo(spawnRoom.Rect.GetCenter()))
            );

            var bossRoom = deadEnds[^1];
            bossRoom.Type = RoomType.Boss;
            deadEnds.Remove(bossRoom);

            var treasuresPlaced = 0;

            foreach (var room in deadEnds)
            {
                if (treasuresPlaced >= maxTreasureRoomsCounts)
                    break;

                if (room == spawnRoom)
                    continue;

                room.Type = RoomType.Treasure;
                treasuresPlaced++;
            }
        }
        else
        {
            var sortedRooms = new List<Room>(rooms);
            sortedRooms.Sort((a, b) =>
                a
                    .Rect.GetCenter()
                    .DistanceSquaredTo(spawnRoom.Rect.GetCenter())
                    .CompareTo(b.Rect.GetCenter().DistanceSquaredTo(spawnRoom.Rect.GetCenter()))
            );

            sortedRooms[^1].Type = RoomType.Boss;
        }
    }

    private Vector2I GetRandomPos(Room room)
    {
        var inner = room.Rect.Grow(-1);

        return new Vector2I(
            rng.RandiRange(inner.Position.X, inner.End.X - 1),
            rng.RandiRange(inner.Position.Y, inner.End.Y - 1)
        );
    }
}