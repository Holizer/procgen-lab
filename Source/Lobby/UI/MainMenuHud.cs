using System.Linq;
using Godot;
using Godot.Collections;
using ProcGenLab.Lobby.Enums;
using ProcGenLab.Lobby.Resources;
using ProcGenLab.Shared.Core;
using ProcGenLab.Shared.UI;

namespace ProcGenLab.Lobby.UI;

public partial class MainMenuHud : Control
{
    [Signal]
    public delegate void GenerationRequestedEventHandler(
        PackedScene scene,
        GenerationConfig config
    );

    [Signal]
    public delegate void MenuToggleRequestedEventHandler();

    private readonly Dictionary<AlgorithmType, BaseConfigPanel> _panels = [];
    private PackedScene _pendingScene;

    [Export] public Array<ProcGenWorldOption> Worlds { get; set; } = [];

    [ExportGroup("Nodes")] [Export] public Control AlgorithmsSelectorContainer { get; set; }

    [Export] public Control ConfigPanelContainer { get; set; }

    [Export] public Button GenerateButton { get; set; }

    [Export] public Button QuitButton { get; set; }

    [Export] public Button BackButton { get; set; }

    public BaseConfigPanel ActiveConfigPanel { get; private set; }

    public override void _Ready()
    {
        BackButton.FocusMode = FocusModeEnum.None;
        GenerateButton.FocusMode = FocusModeEnum.None;
        QuitButton.FocusMode = FocusModeEnum.None;

        InitPanels();
    }

    public void SetMenuVisible(bool visible, bool hasActiveWorld)
    {
        Visible = true;

        AlgorithmsSelectorContainer.Visible = visible;
        QuitButton.Visible = visible;

        ConfigPanelContainer.Visible = visible && ActiveConfigPanel != null;
        GenerateButton.Visible = visible && ActiveConfigPanel != null;

        BackButton.Visible = hasActiveWorld;
    }

    public void OnGeneratePressed()
    {
        if (ActiveConfigPanel is null || _pendingScene is null)
            return;

        var config = ActiveConfigPanel.BuildConfig();
        if (config is null)
            return;

        EmitSignal(SignalName.GenerationRequested, _pendingScene, config);
    }

    public void OnBackPressed()
    {
        EmitSignal(SignalName.MenuToggleRequested);
    }

    private void OnAlgorithmSelected(AlgorithmType type)
    {
        var option = Worlds.FirstOrDefault(world => world.Type == type);
        if (option is null || !_panels.TryGetValue(type, out var targetPanel))
            return;

        foreach (var panel in _panels.Values)
            panel.Visible = false;

        targetPanel.Visible = true;
        _pendingScene = option.Scene;
        ActiveConfigPanel = targetPanel;

        SetMenuVisible(true, BackButton.Visible);
    }

    private void InitPanels()
    {
        foreach (var option in Worlds)
        {
            if (option.ConfigPanel is null || option.DefaultConfig is null)
                continue;

            var node = option.ConfigPanel.Instantiate();
            ConfigPanelContainer.AddChild(node);

            if (node is BaseConfigPanel panel)
            {
                panel.LoadConfig(option.DefaultConfig.Clone<GenerationConfig>());
                panel.Visible = false;
                _panels[option.Type] = panel;
            }
            else
            {
                node.QueueFree();
            }
        }
    }

    public void OnCaButtonPressed()
    {
        OnAlgorithmSelected(AlgorithmType.CellularAutomata);
    }

    public void OnBspButtonPressed()
    {
        OnAlgorithmSelected(AlgorithmType.BinarySpacePartitioning);
    }

    public void OnWfcButtonPressed()
    {
        OnAlgorithmSelected(AlgorithmType.WaveFunctionCollapse);
    }

    public void OnQuitPressed()
    {
        GetTree().Quit();
    }
}