using System.Linq;
using ProcGenLab.CellularAutomata.Models;
using ProcGenLab.CellularAutomata.Resources;
using ProcGenLab.CellularAutomata.Services;
using ProcGenLab.Shared.Core;
using ProcGenLab.Shared.Enums;
using ProcGenLab.Shared.Utils;

namespace ProcGenLab.CellularAutomata.Core;

public class CaMapBuilder : BaseMapBuilder<CaMapBuilder, CaMap, CaConfig>
{
    private readonly RegionAnalyzer _regionAnalyzer = new();

    private readonly RegionConnector _regionConnector = new();

    private readonly AutomataSimulator _simulator = new();

    private BiomeCreator _biomeService;

    private PropsPlacer _propsPlacer;

    protected override CaMap EmptyMap => new(0, 0);

    public CaMapBuilder Initialize(CaConfig config)
    {
        return Step(() =>
        {
            base.Configure(config);
            _biomeService = new BiomeCreator(config.BiomesSettings);
            _propsPlacer = new PropsPlacer(config.PropDatabase);
            Map = new CaMap(config.MapWidth, config.MapHeight);
        });
    }

    public CaMapBuilder GenerateRandomNoise()
    {
        return Step(() =>
        {
            var width = Map.Width;
            var height = Map.Height;
            var fillPercent = Config.FillPercent;

            for (var y = 1; y < height - 1; y++)
            for (var x = 1; x < width - 1; x++)
            {
                var type = Rng.RandiRange(0, 100) < fillPercent ? TileType.Water : TileType.Ground;
                Map.SetTile(x, y, type);
            }
        });
    }

    public CaMapBuilder Evolve()
    {
        return Step(() =>
        {
            for (var i = 0; i < Config.SimulationSteps; i++)
                _simulator.RunStep(Map, Config.WallTransitionThreshold);
        });
    }

    public CaMapBuilder ConnectRegions()
    {
        return Step(() =>
        {
            var regions = _regionAnalyzer.GetRegionsType(Map, TileType.Ground);
            if (regions.Count != 0)
                _regionConnector.Connect(Map, regions);
            else
                this.LogWarning("No ground regions to connect");
        });
    }

    public CaMapBuilder RemoveIslandsAndPockets()
    {
        return Step(() => { _regionAnalyzer.CleanupRegions(Map, Config.MinIslandSizeTiles, Config.MinLakeSizeTiles); });
    }

    public CaMapBuilder CreateBiomes()
    {
        return Step(() =>
        {
            var allRegions = _regionAnalyzer.GetAllRegions(Map);
            _biomeService.AssignBiomes(Map, allRegions, Rng, Config.BiomeNoiseFrequency);

            var biomesMap = Config.BiomesSettings.ToDictionary(biome => biome.Type);
            _propsPlacer.PlaceProps(Map, allRegions, biomesMap, Rng);
        });
    }
}