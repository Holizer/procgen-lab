using ProcGenLab.CellularAutomata.Enums;
using ProcGenLab.Shared.Enums;

namespace ProcGenLab.CellularAutomata.Models;

public struct CaCell(TileType terrain)
{
    public TileType Terrain { get; set; } = terrain;
    public BiomeZone? Biome { get; set; }
    public string PropId { get; set; }
    public bool IsOccupied { get; set; }
}