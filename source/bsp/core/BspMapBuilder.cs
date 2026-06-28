using ProcGenLab.BSP.Models;
using ProcGenLab.BSP.Resources;
using ProcGenLab.BSP.Services;
using ProcGenLab.Shared.Core;
using ProcGenLab.Shared.Utils;

namespace ProcGenLab.BSP.Core;

public class BspMapBuilder : BaseMapBuilder<BspMapBuilder, BspMap, BspConfig>
{
    private readonly CorridorBuilder _corridorBuilder = new();

    private RoomDecorator _roomDecorator;

    private RoomService _roomService;

    private BspNode _root;

    protected override BspMap EmptyMap => new(0, 0);

    public BspMapBuilder Initialize(BspConfig config)
    {
        return Step(() =>
        {
            base.Configure(config);
            Map = new BspMap(config.MapWidth, config.MapHeight);
            _roomService ??= new RoomService(Rng);
            _roomDecorator ??= new RoomDecorator(Rng);
            _root = null;
        });
    }

    public BspMapBuilder SplitSpace()
    {
        return Step(() =>
        {
            _root =
                new BspProcessor(Rng).GeneratePartitionTree(Config)
                ?? throw new LabException(this, "Failed to split space: Root node is null");
        });
    }

    public BspMapBuilder AddRooms()
    {
        return Step(() =>
        {
            if (_root == null)
                throw new LabException(this, "Root is null. Call SplitSpace() first.");

            _roomService.CreateRooms(Map, _root, Config);

            if (Map.Rooms.Count == 0)
                this.LogWarning("No rooms created");
        });
    }

    public BspMapBuilder ConnectRooms()
    {
        return Step(() =>
        {
            if (_root == null)
                throw new LabException(this, "Root is null. Call SplitSpace() first.");

            _corridorBuilder.Connect(_root, Map);
        });
    }

    public BspMapBuilder AssignRoomRoles()
    {
        return Step(() =>
        {
            if (Map.Rooms.Count == 0)
                throw new LabException(this, "No rooms to assign roles to.");

            _roomDecorator.AssignTypes(Map.Rooms, _corridorBuilder, Config.MaxTreasureRooms);
            _roomDecorator.PlaceObjects(Map.Rooms);
            _roomDecorator.PlaceEnemies(Map.Rooms, Config.EnemyConfig);
        });
    }
}