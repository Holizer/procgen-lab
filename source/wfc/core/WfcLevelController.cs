using Godot;
using ProcGenLab.Shared.Controllers;
using ProcGenLab.Shared.Diagnostics;
using ProcGenLab.Shared.Utils;
using ProcGenLab.WFC.Resources;
using ProcGenLab.WFC.Visualization;

namespace ProcGenLab.WFC.Core;

public partial class WfcLevelController : ConfigurableLevelController<WfcConfig>
{
    [Export] public WfcMapVisualizer Visualizer { get; set; }

    [Export] public WfcChunkCatalog Catalog { get; set; }

    [Export] public WfcConfig DevConfig { get; set; }

    protected override WfcConfig DefaultConfig => DevConfig;

    protected override void CreateLevel()
    {
        var wfcBuilder = new WfcMapBuilder();
        var (timeMs, memoryMb, map) = BenchmarkService.Profile(
            static state =>
            {
                var (builder, config, catalog) = state;

                var pipeline = builder.Initialize(config, catalog);

                if (config.UseBspTopology)
                    pipeline = pipeline.ApplyBspTopology();

                return pipeline.Solve().Build();
            },
            (builder: wfcBuilder, Config, Catalog)
        );

        if (wfcBuilder.IsFaulted || map == null)
        {
            this.LogError("Generation failed at one of the steps");

            return;
        }

        DiagnosticsBus.Emit("WFC", timeMs, memoryMb, Config.Seed);

        Visualizer.Render(
            new WfcRenderContext(
                map,
                wfcBuilder.Registry.Tiles,
                Config.MacroCellSizePixels
            )
        );
    }
}