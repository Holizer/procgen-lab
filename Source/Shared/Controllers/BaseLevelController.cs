using Godot;
using ProcGenLab.Shared.Core;

namespace ProcGenLab.Shared.Controllers;

public abstract partial class BaseLevelController : Node2D
{
    protected abstract void CreateLevel();

    public override void _Ready()
    {
        CreateLevel();
    }

    public override void _Input(InputEvent @event)
    {
        if (OS.IsDebugBuild() && @event.IsActionPressed(GameInput.RegenerateLevel) && !@event.IsEcho())
            CreateLevel();
    }
}