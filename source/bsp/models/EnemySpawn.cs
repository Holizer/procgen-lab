using Godot;
using ProcGenLab.BSP.Enums;

namespace ProcGenLab.BSP.Models;

public record EnemySpawn(EnemyType Type, Vector2I Position);