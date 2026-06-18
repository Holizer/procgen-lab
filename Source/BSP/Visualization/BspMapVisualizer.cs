using Godot;
using ProcGenLab.Shared.Visualization;

namespace ProcGenLab.BSP.Visualization;

public partial class BspMapVisualizer : BaseMapVisualizer<BspMapRenderContext>
{
    public override void Render(BspMapRenderContext context)
    {
        var map = context.BspMap;
        Clear();
        DrawTerrain(map);
        DrawTerrainSolidBorder(map);

        DrawObjects(map);
        if (ShowDebugLabels)
            DrawDebugInfo(map);
    }

    private void Clear()
    {
        TerrainLayer.Clear();
        ClearContainer(ObjectsContainer);
        ClearContainer(EnemiesContainer);
        ClearContainer(DebugContainer);
    }

    private static void ClearContainer(Node2D container)
    {
        if (container == null)
            return;

        foreach (var child in container.GetChildren())
            child.QueueFree();
    }
}