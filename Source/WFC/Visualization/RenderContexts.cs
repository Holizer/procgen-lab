using System.Collections.Generic;
using ProcGenLab.Shared.Core;
using ProcGenLab.Shared.Visualization;
using ProcGenLab.WFC.Enums;
using ProcGenLab.WFC.Resources;

namespace ProcGenLab.WFC.Visualization;

public record WfcRenderContext(
    Grid2D Map,
    IReadOnlyDictionary<MacroTileType, MacroTile> Tiles,
    int MacroCellSizePixels
) : MapRenderContext(Map);