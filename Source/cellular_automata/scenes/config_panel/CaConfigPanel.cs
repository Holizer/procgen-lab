using ProcGenLab.CellularAutomata.Resources;
using ProcGenLab.Shared.Core;
using ProcGenLab.Shared.UI;

namespace ProcGenLab.CellularAutomata.Scenes;

public partial class CaConfigPanel : BaseConfigPanel
{
    private readonly ConfigBinder<CaConfig> _binder;

    public CaConfigPanel()
    {
        _binder = new ConfigBinder<CaConfig>(this);
    }

    public override void LoadConfig(GenerationConfig defaults)
    {
        _binder.LoadConfig(defaults);
    }

    public override GenerationConfig BuildConfig()
    {
        return _binder.BuildConfig();
    }
}