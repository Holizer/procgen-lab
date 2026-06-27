using Godot;

namespace ProcGenLab.WFC.Resources;

[GlobalClass]
public partial class WfcWeightConfig : Resource
{
    [Export] public WfcWeightEntry[] Weights { get; set; }
}