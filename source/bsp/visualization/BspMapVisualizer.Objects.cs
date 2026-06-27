using Godot;
using Godot.Collections;
using ProcGenLab.BSP.Enums;
using ProcGenLab.BSP.Models;
using ProcGenLab.Shared.Visualization;

namespace ProcGenLab.BSP.Visualization;

public partial class BspMapVisualizer : BaseMapVisualizer<BspMapRenderContext>
{
    [ExportGroup("Enemies and Prefabs Configuration")]
    [Export]
    public Node2D ObjectsContainer { get; set; }

    [Export] public Node2D EnemiesContainer { get; set; }

    [Export] public Dictionary<EnemyType, PackedScene> EnemyPrefabs { get; set; } = new();

    [Export] public Dictionary<ObjectType, PackedScene> ObjectPrefabs { get; set; } = new();

    private void DrawObjects(BspMap map)
    {
        if (ObjectsContainer == null || EnemiesContainer == null)
            return;

        foreach (var room in map.Rooms)
        {
            foreach (var obj in room.Objects)
                if (ObjectPrefabs.TryGetValue(obj.Type, out var prefab))
                    SpawnPrefab(ObjectsContainer, obj.Position, prefab);

            foreach (var enemy in room.Enemies)
                if (EnemyPrefabs.TryGetValue(enemy.Type, out var prefab))
                    SpawnPrefab(EnemiesContainer, enemy.Position, prefab);
        }
    }

    private void SpawnPrefab(Node2D parent, Vector2I mapPos, PackedScene prefab)
    {
        if (prefab == null)
            return;

        var instance = prefab.Instantiate<Node2D>();
        instance.Position = TerrainLayer.MapToLocal(mapPos);
        parent.AddChild(instance);
    }
}