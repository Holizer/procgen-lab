using Godot;
using ProcGenLab.Shared.Core;

namespace ProcGenLab.Shared.UI;

public abstract partial class BaseConfigPanel : Control
{
    [Export] public PackedScene ConfigRowScene { get; set; }

    private GenerationConfig CurrentConfig { get; set; }

    public abstract GenerationConfig BuildConfig();

    public abstract void LoadConfig(GenerationConfig defaults);
}