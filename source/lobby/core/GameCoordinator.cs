using Godot;
using ProcGenLab.Lobby.UI;
using ProcGenLab.Shared.Core;

namespace ProcGenLab.Lobby.Core;

public partial class GameCoordinator : Node
{
    private bool _hasActiveWorld;
    private bool _isMenuOpen = true;
    private GenerationConfig _lastConfig;

    private PackedScene _lastScene;

    [Export] public MainMenuHud MainMenuHud { get; set; }

    [Export] public PerformanceMonitor PerformanceMonitor { get; set; }

    [Export] public Control RegenerateLevelHint { get; set; }

    [Export] public LevelStage LevelStage { get; set; }

    public override void _Ready()
    {
        MainMenuHud.GenerationRequested += OnGenerationRequested;
        MainMenuHud.MenuToggleRequested += ToggleMenu;

        UpdateState();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!_hasActiveWorld)
            return;

        if (@event.IsActionPressed(GameInput.UiCancel))
        {
            ToggleMenu();
            GetViewport().SetInputAsHandled();
        }
        else if (!_isMenuOpen && @event.IsActionPressed(GameInput.RegenerateLevel))
        {
            RegenerateCurrentWorld();
            GetViewport().SetInputAsHandled();
        }
    }

    private void ToggleMenu()
    {
        _isMenuOpen = !_isMenuOpen;
        UpdateState();
    }

    private void OnGenerationRequested(PackedScene scene, GenerationConfig config)
    {
        _lastScene = scene;
        _lastConfig = config;
        _hasActiveWorld = true;
        _isMenuOpen = false;

        UpdateState();
        LoadLevel();
    }

    private void RegenerateCurrentWorld()
    {
        if (_lastScene is null || MainMenuHud.ActiveConfigPanel is null)
            return;

        _lastConfig = MainMenuHud.ActiveConfigPanel.BuildConfig();
        if (_lastConfig is null)
            return;

        LoadLevel();
    }

    private void LoadLevel()
    {
        LevelStage.LoadLevel(_lastScene, _lastConfig);
    }

    private void UpdateState()
    {
        LevelStage.ProcessMode = _isMenuOpen ? ProcessModeEnum.Disabled : ProcessModeEnum.Inherit;

        MainMenuHud.SetMenuVisible(_isMenuOpen, _hasActiveWorld);
        RegenerateLevelHint.Visible = !_isMenuOpen && _hasActiveWorld;
        PerformanceMonitor.Visible = !_isMenuOpen && _hasActiveWorld;
    }
}