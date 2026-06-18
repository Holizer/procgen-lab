using ProcGenLab.Shared.Core;

namespace ProcGenLab.Shared.Interfaces;

public interface IConfigurableLevel
{
    void ApplyConfig(GenerationConfig config);
}