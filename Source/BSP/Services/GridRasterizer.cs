using ProcGenLab.BSP.Models;
using ProcGenLab.Shared.Enums;

namespace ProcGenLab.BSP.Services;

public class GridRasterizer
{
    public void Rasterize(BspMap map)
    {
        RasterizeRooms(map);
        RasterizeCorridors(map);
    }

    private void RasterizeRooms(BspMap map)
    {
        foreach (var room in map.Rooms)
            for (var y = room.Rect.Position.Y; y < room.Rect.End.Y; y++)
            for (var x = room.Rect.Position.X; x < room.Rect.End.X; x++)
                map.SetTile(x, y, TileType.Floor);
    }

    private void RasterizeCorridors(BspMap map)
    {
        foreach (var tile in map.PathsTiles)
            map.SetTile(tile.X, tile.Y, TileType.Floor);
    }
}