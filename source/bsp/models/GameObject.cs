using Godot;
using ProcGenLab.BSP.Enums;

namespace ProcGenLab.BSP.Models;

public readonly record struct GameObject(ObjectType Type, Vector2I Position);