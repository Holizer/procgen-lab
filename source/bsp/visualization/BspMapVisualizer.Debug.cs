using Godot;
using Godot.Collections;
using ProcGenLab.BSP.Enums;
using ProcGenLab.BSP.Models;
using ProcGenLab.Shared.Visualization;

namespace ProcGenLab.BSP.Visualization;

public partial class BspMapVisualizer : BaseMapVisualizer<BspMapRenderContext>
{
    [Export] public bool ShowDebugLabels = true;

    [ExportGroup("Debug Configuration")]
    [Export]
    public Node2D DebugContainer { get; set; }

    [Export] public LabelSettings DebugLabelStyle { get; set; }

    [Export] public Dictionary<RoomType, Color> RoomColors { get; set; } = new();

    private void DrawDebugInfo(BspMap map)
    {
        if (DebugContainer == null)
            return;

        foreach (var room in map.Rooms)
        {
            LabelSettings uniqueSettings = null;

            if (DebugLabelStyle != null)
            {
                uniqueSettings = (LabelSettings)DebugLabelStyle.Duplicate();
                if (RoomColors.TryGetValue(room.Type, out var labelColor))
                    uniqueSettings.FontColor = labelColor;
            }

            var label = new Label
            {
                Text = $"Type: {room.Type}\nSize: {room.Rect.Size.X}x{room.Rect.Size.Y}",
                LabelSettings = uniqueSettings,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Position = TerrainLayer.MapToLocal(room.Rect.GetCenter()),
                GrowHorizontal = Control.GrowDirection.Both,
                GrowVertical = Control.GrowDirection.Both
            };

            DebugContainer.AddChild(label);
        }
    }
}