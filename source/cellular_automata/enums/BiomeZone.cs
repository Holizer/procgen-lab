namespace ProcGenLab.CellularAutomata.Enums;

public enum BiomeZone
{
    Clearing = 0,

    Sparse = 1,

    ConiferousForest = 2,

    OakForest = 3,

    MixedForest = 4,

    Lake = 10,

    Sea = 11
}

public static class BiomeZoneExtensions
{
    public static bool IsWater(this BiomeZone zone)
    {
        return zone is BiomeZone.Lake or BiomeZone.Sea;
    }

    public static bool IsLand(this BiomeZone zone)
    {
        return !zone.IsWater();
    }
}