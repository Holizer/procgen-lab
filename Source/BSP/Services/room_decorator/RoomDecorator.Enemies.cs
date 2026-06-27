using System.Collections.Generic;
using Godot;
using ProcGenLab.BSP.Enums;
using ProcGenLab.BSP.Models;
using ProcGenLab.BSP.Resources;

namespace ProcGenLab.BSP.Services;

public partial class RoomDecorator
{
    public void PlaceEnemies(IReadOnlyList<Room> rooms, BspEnemyConfig enemyConfig)
    {
        if (enemyConfig == null)
            return;

        foreach (var room in rooms)
            switch (room.Type)
            {
                case RoomType.Standard:
                    if (enemyConfig.StandardGroups == null || enemyConfig.StandardGroups.Count == 0)
                        break;
                    var index = rng.RandiRange(0, enemyConfig.StandardGroups.Count - 1);
                    PlaceGroup(room, enemyConfig.StandardGroups[index], rng);
                    break;

                case RoomType.Boss:
                    if (enemyConfig.BossGroup == null)
                        break;
                    PlaceGroup(room, enemyConfig.BossGroup, rng);
                    break;
            }
    }

    private void PlaceGroup(Room room, EnemyGroup group, RandomNumberGenerator rng)
    {
        var inner = room.Rect.Grow(-1);

        foreach (var entry in group.Entries)
            for (var i = 0; i < entry.Count; i++)
            {
                var pos = new Vector2I(
                    rng.RandiRange(inner.Position.X, inner.End.X - 1),
                    rng.RandiRange(inner.Position.Y, inner.End.Y - 1)
                );
                room.AddEnemy(new EnemySpawn(entry.Type, pos));
            }
    }
}