namespace ProcGenLab.Shared.Interfaces;

public interface IBspConfig : IMapConfig
{
    int MaxDepth { get; }
    float AspectRatioThreshold { get; }
    int MinSplitSize { get; }
}