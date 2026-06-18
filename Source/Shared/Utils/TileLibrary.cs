using System.Collections.Generic;
using Godot;

namespace ProcGenLab.Shared.Utils;

public class TileLibrary
{
    private readonly Dictionary<string, int> _sourceByTag = new();
    private readonly Dictionary<string, List<Vector2I>> _tilesByTag = new();

    public TileLibrary(TileSet tileSet, string layerName)
    {
        for (var i = 0; i < tileSet.GetSourceCount(); i++)
        {
            var sourceId = tileSet.GetSourceId(i);
            var source = tileSet.GetSource(sourceId) as TileSetAtlasSource;
            if (source == null)
                continue;

            for (var j = 0; j < source.GetTilesCount(); j++)
            {
                var coords = source.GetTileId(j);
                var data = source.GetTileData(coords, 0);
                var tag = data.GetCustomData(layerName).AsString();
                if (string.IsNullOrEmpty(tag))
                    continue;

                if (!_tilesByTag.TryGetValue(tag, out var list))
                {
                    list = new List<Vector2I>();
                    _tilesByTag[tag] = list;
                    _sourceByTag[tag] = sourceId;
                }

                list.Add(coords);
            }
        }
    }

    public (Vector2I AtlasCoords, int SourceId) GetRandomTile(string tag)
    {
        var (isExist, list) = TryGetTilesIfExist(tag);
        if (!isExist)
            return (GridUtils.InvalidTile, 0);

        return (list[GD.RandRange(0, list.Count - 1)], _sourceByTag[tag]);
    }

    public (Vector2I AtlasCoords, int SourceId) GetFirstTile(string tag)
    {
        var (isExist, list) = TryGetTilesIfExist(tag);

        if (!isExist)
            return (GridUtils.InvalidTile, 0);

        return (list[0], _sourceByTag[tag]);
    }

    public bool HasTag(string tag)
    {
        return _tilesByTag.ContainsKey(tag);
    }

    private (bool, List<Vector2I>) TryGetTilesIfExist(string tag)
    {
        if (!_tilesByTag.TryGetValue(tag, out var list) || list.Count == 0)
        {
            this.LogError($"Tag '{tag}' not found!");
            return (false, null);
        }

        return (true, list);
    }
}