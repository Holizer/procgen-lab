using ProcGenLab.BSP.Services;
using ProcGenLab.Shared.Core;
using ProcGenLab.Shared.Utils;
using ProcGenLab.WFC.Models;
using ProcGenLab.WFC.Resources;
using ProcGenLab.WFC.Services;

namespace ProcGenLab.WFC.Core;

public class WfcBuilder : BaseMapBuilder<WfcBuilder, WfcMap, WfcConfig>
{
    protected override WfcMap EmptyMap => new(0, 0, []);
    public MacroRegistry Registry { get; private set; }

    public WfcBuilder Initialize(WfcConfig config, WfcChunkCatalog catalog)
    {
        return Step(() =>
        {
            base.Configure(config);
            Registry = new MacroRegistry(catalog, config.Weights);
            Map = new WfcMap(Config.MapWidth, Config.MapHeight, Registry.Tiles.Keys);
        });
    }

    public WfcBuilder ApplyBspTopology()
    {
        return Step(() =>
        {
            var root = new BspProcessor(Rng).GeneratePartitionTree(Config);
            var topology = BspToTopologyConverter.Convert(root);
            Map.Topology = topology;
            TopologyPlacer.Apply(topology, Map);
        });
    }

    public WfcBuilder Solve()
    {
        return Step(() =>
        {
            var solver = new WfcSolver(Map, Registry, Rng);
            var success = solver.Solve();

            if (!success)
                throw new LabException(this, "Failed to find a valid tile configuration.");
        });
    }
}