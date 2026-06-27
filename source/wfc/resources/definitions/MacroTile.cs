using Godot;
using ProcGenLab.Shared.Enums;
using ProcGenLab.WFC.Enums;
using ProcGenLab.WFC.Services;

namespace ProcGenLab.WFC.Resources;

[GlobalClass]
public partial class MacroTile : Resource
{
    [Export] public MacroTileType Type { get; set; }

    [Export] public PackedScene ChunkScene { get; set; }

    public MacroSocketType GetSocket(Direction dir)
    {
        return MacroTileSocketMap.Get(Type, dir);
    }
}