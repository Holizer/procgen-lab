using System.Collections.Generic;

namespace ProcGenLab.Shared.Utils;

public class DisjointSet
{
    private readonly Dictionary<int, int> _parent = new();

    public DisjointSet(IEnumerable<int> ids)
    {
        foreach (var id in ids)
            _parent[id] = id;
    }

    public bool Union(int i, int j)
    {
        var rootI = Find(i);
        var rootJ = Find(j);

        if (rootI == rootJ)
            return false;

        _parent[rootI] = rootJ;
        return true;
    }

    private int Find(int id)
    {
        if (!_parent.TryGetValue(id, out var parentId))
        {
            _parent[id] = id;
            return id;
        }

        if (parentId == id)
            return id;

        return _parent[id] = Find(parentId);
    }
}