using ProcGenLab.BSP.Resources;
using ProcGenLab.Shared.Core;
using ProcGenLab.Shared.UI;

namespace ProcGenLab.BSP.Scenes;

public partial class BspConfigPanel : BaseConfigPanel
{
    private readonly ConfigBinder<BspConfig> _binder;

    public BspConfigPanel()
    {
        _binder = new ConfigBinder<BspConfig>(this);
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