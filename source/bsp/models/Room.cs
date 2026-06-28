using System.Collections.Generic;
using Godot;
using ProcGenLab.BSP.Enums;

namespace ProcGenLab.BSP.Models;

public class Room(Rect2I rect)
{
    private readonly List<EnemySpawn> _enemies = [];

    private readonly List<GameObject> _objects = [];

    public Rect2I Rect { get; } = rect;

    public RoomType Type { get; set; } = RoomType.Standard;

    public IReadOnlyList<GameObject> Objects => _objects;

    public IReadOnlyList<EnemySpawn> Enemies => _enemies;

    public void AddObject(GameObject obj)
    {
        _objects.Add(obj);
    }

    public void AddEnemy(EnemySpawn enemy)
    {
        _enemies.Add(enemy);
    }
}