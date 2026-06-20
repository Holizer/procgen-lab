using Godot;
using ProcGenLab.Shared.Interfaces;

namespace ProcGenLab.Shared.Core;

[GlobalClass]
public partial class GenerationConfig : Resource, ISeedableConfig
{
    [ExportGroup("Grid Settings")]
    [Export]
    public int TileSize { get; set; } = 16;

    [ExportGroup("Seed Settings")]
    [Export]
    public bool UseRandomSeed { get; set; } = true;

    [Export(PropertyHint.Range, "0, 2147483647, 1")]
    public int Seed { get; set; }

    public RandomNumberGenerator CreateRng()
    {
        var rng = new RandomNumberGenerator();
        rng.Seed = UseRandomSeed ? (ulong)GD.RandRange(0, int.MaxValue) : (ulong)Seed;
        return rng;
    }

    public T Clone<T>()
        where T : GenerationConfig
    {
        return (T)Duplicate(true);
    }
}