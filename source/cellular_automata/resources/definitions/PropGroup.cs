using Godot;
using Godot.Collections;
using ProcGenLab.CellularAutomata.Enums;

namespace ProcGenLab.CellularAutomata.Resources;

public partial class PropGroup : Resource
{
    [Export] public PropType Type;

    [Export] public Array<PropData> Variations;
}