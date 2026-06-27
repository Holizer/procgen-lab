using ProcGenLab.BSP.Models;
using ProcGenLab.BSP.Resources;
using ProcGenLab.Shared.Visualization;

namespace ProcGenLab.BSP.Visualization;

public record BspMapRenderContext(BspMap BspMap, BspEnemyConfig EnemyConfig) : MapRenderContext(BspMap);