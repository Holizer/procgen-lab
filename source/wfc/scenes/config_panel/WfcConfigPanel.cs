using Godot;
using ProcGenLab.Shared.Core;
using ProcGenLab.Shared.UI;

namespace ProcGenLab.WFC.Scenes;

public partial class WfcConfigPanel : BaseConfigPanel
{
    private readonly WfcConfigBinder _binder;

    public WfcConfigPanel()
    {
        _binder = new WfcConfigBinder(this);
    }

    [Export] public ScrollPanel WeightsRowsContainer { get; set; }

    [Export] public Control BspSettingsContainer { get; set; }

    public override void LoadConfig(GenerationConfig defaults)
    {
        _binder.LoadConfig(defaults);
    }

    public override GenerationConfig BuildConfig()
    {
        return _binder.BuildConfig();
    }

    public void OnTopologyToggled(bool enabled)
    {
        WeightsRowsContainer.Visible = !enabled;
        BspSettingsContainer.Visible = enabled;
    }
}