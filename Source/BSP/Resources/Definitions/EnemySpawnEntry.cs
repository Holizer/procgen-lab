using Godot;
using ProcGenLab.BSP.Enums;

namespace ProcGenLab.BSP.Resources;

[GlobalClass]
public partial class EnemySpawnEntry : Resource
{
    [Export] public EnemyType Type { get; set; }

    [Export(PropertyHint.Range, "1, 10, 1")]
    public int Count { get; set; } = 1;
}