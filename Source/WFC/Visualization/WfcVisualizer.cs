using System.Collections.Generic;
using Godot;
using ProcGenLab.Shared.Visualization;
using ProcGenLab.WFC.Enums;
using ProcGenLab.WFC.Models;
using ProcGenLab.WFC.Resources;

namespace ProcGenLab.WFC.Visualization;

public partial class WfcVisualizer : BaseMapVisualizer<WfcRenderContext>
{
    private int _cellPx;
    private Node2D _chunkScenesRoot;

    [Export] public bool DebugMode { get; set; }

    public override void _Ready()
    {
        base._Ready();
        EnsureChunkScenesRoot();
    }

    public override void Render(WfcRenderContext context)
    {
        var map = (WfcMap)context.Map;
        var tiles = context.Tiles;
        _cellPx = context.MacroCellSizePixels;

        EnsureChunkScenesRoot();
        RebuildChunkScenes(map, tiles);
    }

    private void EnsureChunkScenesRoot()
    {
        if (IsInstanceValid(_chunkScenesRoot))
            return;

        _chunkScenesRoot = new Node2D { Name = "ChunkScenes" };
        AddChild(_chunkScenesRoot);
    }

    private void RebuildChunkScenes(WfcMap map, IReadOnlyDictionary<MacroTileType, MacroTile> tiles)
    {
        ClearChunkScenes();

        for (var y = 0; y < map.Height; y++)
        for (var x = 0; x < map.Width; x++)
        {
            var cell = map[x, y];

            if (!cell.IsCollapsed)
                continue;

            if (cell.CollapsedType is not MacroTileType type)
                continue;

            if (!tiles.TryGetValue(type, out var tile))
                continue;

            var instance = tile.ChunkScene.Instantiate<Node2D>();

            instance.Position = GetCellWorldPosition(x, y);

            _chunkScenesRoot.AddChild(instance);

            if (DebugMode)
            {
                var label = new Label();
                label.Text = type.ToString();
                label.Position = GetCellWorldPosition(x, y);
                label.AddThemeColorOverride("font_color", Colors.Yellow);
                _chunkScenesRoot.AddChild(label);
            }
        }
    }

    private Vector2 GetCellWorldPosition(int x, int y)
    {
        return new Vector2(x * _cellPx, y * _cellPx);
    }

    private void ClearChunkScenes()
    {
        foreach (var child in _chunkScenesRoot.GetChildren())
            child.QueueFree();
    }
}