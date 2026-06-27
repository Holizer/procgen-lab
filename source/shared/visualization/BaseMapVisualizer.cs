using Godot;

namespace ProcGenLab.Shared.Visualization;

public abstract partial class BaseMapVisualizer<TContext> : Node2D
{
    public abstract void Render(TContext context);
}