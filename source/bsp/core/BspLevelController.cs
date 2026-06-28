using Godot;
using ProcGenLab.BSP.Resources;
using ProcGenLab.BSP.Visualization;
using ProcGenLab.Shared.Controllers;
using ProcGenLab.Shared.Diagnostics;
using ProcGenLab.Shared.Utils;

namespace ProcGenLab.BSP.Core;

public partial class BspLevelController : ConfigurableLevelController<BspConfig>
{
    [Export] public BspMapVisualizer Visualizer { get; set; }

    [Export] public BspConfig DevConfig { get; set; }

    protected override BspConfig DefaultConfig => DevConfig;

    protected override void CreateLevel()
    {
        var bspBuilder = new BspMapBuilder();
        var (timeMs, memoryMb, map) = BenchmarkService.Profile(
            static state =>
            {
                var (builder, config) = state;

                return builder
                    .Initialize(config)
                    .SplitSpace()
                    .AddRooms()
                    .ConnectRooms()
                    .AssignRoomRoles()
                    .Build();
            },
            (Builder: bspBuilder, Config)
        );

        if (bspBuilder.IsFaulted || map is null)
        {
            this.LogError("Generation failed at one of the steps");

            return;
        }

        DiagnosticsBus.Emit("BSP", timeMs, memoryMb, Config.Seed);

        Visualizer.Render(new BspMapRenderContext(map, Config.EnemyConfig));
    }
}