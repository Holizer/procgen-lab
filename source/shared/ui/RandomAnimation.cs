using Godot;

namespace ProcGenLab.Shared.UI;

public partial class RandomAnimation : Node2D
{
    [Export] public AnimatedSprite2D Sprite { get; set; }

    public override void _Ready()
    {
        if (Sprite == null)
            return;

        var animations = Sprite.SpriteFrames.GetAnimationNames();
        Sprite.Play(animations[GD.RandRange(0, animations.Length - 1)]);
    }
}