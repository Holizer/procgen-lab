using Godot;
using ProcGenLab.Shared.Core;
using ProcGenLab.Shared.Interfaces;

namespace ProcGenLab.Lobby.Core;

public partial class LevelStage : Node2D
{
    private Node _currentLevel;

    public void LoadLevel(PackedScene scene, GenerationConfig config)
    {
        _currentLevel?.QueueFree();

        var level = scene.Instantiate();
        if (level is IConfigurableLevel configurable)
            configurable.ApplyConfig(config);

        AddChild(level);
        _currentLevel = level;
    }
}