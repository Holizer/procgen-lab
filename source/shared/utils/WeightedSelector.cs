using System;
using System.Collections.Generic;
using Godot;

namespace ProcGenLab.Shared.Utils;

public static class WeightedSelector
{
    public static T Pick<T>(
        IList<T> items,
        Func<T, int> weightSelector,
        RandomNumberGenerator rng
    )
    {
        var totalWeight = 0;
        for (var i = 0; i < items.Count; i++)
            totalWeight += weightSelector(items[i]);

        if (totalWeight <= 0)
            return default;

        var roll = rng.RandiRange(0, totalWeight - 1);
        var cumulative = 0;

        for (var i = 0; i < items.Count; i++)
        {
            cumulative += weightSelector(items[i]);

            if (roll < cumulative)
                return items[i];
        }

        return items[items.Count - 1];
    }
}