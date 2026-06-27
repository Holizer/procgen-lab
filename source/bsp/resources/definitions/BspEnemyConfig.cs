using Godot;
using Godot.Collections;

namespace ProcGenLab.BSP.Resources;

[GlobalClass]
public partial class BspEnemyConfig : Resource
{
    [ExportGroup("Standard Room")]
    [Export]
    public Array<EnemyGroup> StandardGroups { get; set; } = new();

    [ExportGroup("Boss Room")] [Export] public EnemyGroup BossGroup { get; set; }
}