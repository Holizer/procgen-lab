using Godot;

namespace ProcGenLab.Lobby.UI;

public partial class CursorSprite : Sprite2D
{
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Hidden;
    }

    public override void _Process(double delta)
    {
        Position = GetViewport().GetMousePosition();
    }
}