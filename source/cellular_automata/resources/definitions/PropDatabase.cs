using System.Linq;
using Godot;
using Godot.Collections;
using ProcGenLab.CellularAutomata.Enums;
using ProcGenLab.Shared.Utils;

namespace ProcGenLab.CellularAutomata.Resources;

[GlobalClass]
public partial class PropDatabase : Resource
{
    [Export] public Array<PropGroup> Groups { get; set; } = [];

    public PropData GetRandomVariation(PropType type, RandomNumberGenerator rng)
    {
        var group = Groups.FirstOrDefault(g => g.Type == type);

        if (group?.Variations == null || group.Variations.Count == 0)
            return null;

        return WeightedSelector.Pick(group.Variations, variant => variant.Weight, rng);
    }
}