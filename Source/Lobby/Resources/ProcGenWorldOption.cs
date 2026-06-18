using Godot;
using ProcGenLab.Lobby.Enums;
using ProcGenLab.Shared.Core;

namespace ProcGenLab.Lobby.Resources;

[GlobalClass]
public partial class ProcGenWorldOption : Resource
{
    [Export] public AlgorithmType Type { get; set; }

    [Export] public PackedScene Scene { get; set; }

    [Export] public PackedScene ConfigPanel { get; set; }

    [Export] public GenerationConfig DefaultConfig { get; set; }
}