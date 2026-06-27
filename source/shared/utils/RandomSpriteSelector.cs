using System;
using Godot;

namespace ProcGenLab.Shared.Utils;

public partial class RandomSpriteSelector : Node2D
{
    [Export] public Vector2 PivotOffset = Vector2.Zero;

    [Export] public Sprite2D Sprite;

    [Export] public Texture2D[] Variants = Array.Empty<Texture2D>();

    public override void _Ready()
    {
        if (Variants.Length == 0)
            return;

        Sprite.Texture = Variants[GD.RandRange(0, Variants.Length - 1)];
        Sprite.Offset = PivotOffset;
    }
}