using Godot;

namespace ProcGenLab.WFC.Resources;

[GlobalClass]
public partial class WfcChunkCatalog : Resource
{
    [Export] public MacroTile[] Tiles { get; set; }
}