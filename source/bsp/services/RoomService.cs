using System.Collections.Generic;
using Godot;
using ProcGenLab.BSP.Models;
using ProcGenLab.BSP.Resources;

namespace ProcGenLab.BSP.Services;

public class RoomService(RandomNumberGenerator rng)
{
    public void CreateRooms(BspMap map, BspNode node, BspConfig config)
    {
        var rooms = new List<Room>();
        GenerateRooms(node, config, rooms);
        map.Rooms = rooms;
    }

    private void GenerateRooms(BspNode node, BspConfig config, List<Room> roomsList)
    {
        if (!node.IsLeaf)
        {
            GenerateRooms(node.Left!, config, roomsList);
            GenerateRooms(node.Right!, config, roomsList);
            return;
        }

        var area = node.Area;
        var padding = config.PaddingTiles;
        var minW = config.MinRoomWidthTiles;
        var minH = config.MinRoomHeightTiles;

        var maxWidth = area.Size.X - padding * 2;
        var maxHeight = area.Size.Y - padding * 2;

        var finalMinW = Mathf.Clamp(minW, 1, Mathf.Max(1, maxWidth));
        var finalMinH = Mathf.Clamp(minH, 1, Mathf.Max(1, maxHeight));

        var roomW = rng.RandiRange(finalMinW, Mathf.Max(finalMinW, maxWidth));
        var roomH = rng.RandiRange(finalMinH, Mathf.Max(finalMinH, maxHeight));

        var minX = area.Position.X + padding;
        var maxX = area.End.X - padding - roomW;
        var roomX = maxX > minX ? rng.RandiRange(minX, maxX) : minX;

        var minY = area.Position.Y + padding;
        var maxY = area.End.Y - padding - roomH;
        var roomY = maxY > minY ? rng.RandiRange(minY, maxY) : minY;

        node.Room = new Room(new Rect2I(roomX, roomY, roomW, roomH));
        roomsList.Add(node.Room);
    }
}