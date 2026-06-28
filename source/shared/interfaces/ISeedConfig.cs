namespace ProcGenLab.Shared.Interfaces;

public interface ISeedableConfig
{
    int Seed { get; set; }

    bool UseRandomSeed { get; set; }
}