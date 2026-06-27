using Godot;
using Godot.Collections;

namespace ProcGenLab.BSP.Resources;

[GlobalClass]
public partial class EnemyGroup : Resource
{
    [Export] public Array<EnemySpawnEntry> Entries { get; set; } = new();
}