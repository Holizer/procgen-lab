using Godot;
using ProcGenLab.CellularAutomata.Resources;
using ProcGenLab.CellularAutomata.Visualization;
using ProcGenLab.Shared.Controllers;
using ProcGenLab.Shared.Diagnostics;
using ProcGenLab.Shared.Utils;

namespace ProcGenLab.CellularAutomata.Core;

public partial class CaLevelController : ConfigurableLevelController<CaConfig>
{
    [Export] public CaMapVisualizer Visualizer { get; set; }
    [Export] public CaConfig DevConfig { get; set; }

    protected override CaConfig DefaultConfig => DevConfig;

    public override void _Ready()
    {
        Visualizer.Initialize();
        base._Ready();
    }

    protected override void CreateLevel()
    {
        var caBuilder = new CaMapBuilder();
        var (timeMs, memoryMb, map) = BenchmarkService.Profile(
            static state =>
            {
                var (builder, config) = state;
                return builder
                    .Initialize(config)
                    .GenerateRandomNoise()
                    .Evolve()
                    .RemoveIslandsAndPockets()
                    .ConnectRegions()
                    .CreateBiomes()
                    .Build();
            },
            (Builder: caBuilder, Config)
        );

        if (caBuilder.IsFaulted || map == null)
        {
            this.LogError("Generation failed at one of the steps");
            return;
        }

        DiagnosticsBus.Emit("CA", timeMs, memoryMb, Config.Seed);

        Visualizer.Render(new СaMapRenderContext(map));
    }
}