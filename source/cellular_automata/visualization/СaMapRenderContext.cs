using ProcGenLab.CellularAutomata.Models;
using ProcGenLab.Shared.Visualization;

namespace ProcGenLab.CellularAutomata.Visualization;

public record СaMapRenderContext(CaMap CaMap) : MapRenderContext(CaMap);