using System;
using ProcGenLab.Shared.Core;
using ProcGenLab.Shared.Interfaces;
using ProcGenLab.Shared.Utils;
using ProcGenLab.Shared.Сontrollers;

namespace ProcGenLab.Shared.Controllers;

public abstract partial class ConfigurableLevelController<TConfig>
    : BaseLevelController,
        IConfigurableLevel
    where TConfig : GenerationConfig
{
    protected TConfig Config { get; private set; }

    protected virtual TConfig DefaultConfig => null;

    public void ApplyConfig(GenerationConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);

        if (config is not TConfig typedConfig)
            throw new ArgumentException(
                $"Expected {typeof(TConfig).Name}, got {config.GetType().Name}."
            );

        Config = typedConfig;
        OnConfigApplied(Config);
    }

    public override void _Ready()
    {
        switch (Config)
        {
            case null when DefaultConfig is not null:
                Config = DefaultConfig;
                OnConfigApplied(Config);

                break;
            case null:
                this.LogWarning($"{Name}: no config. Override DefaultConfig for F6 testing.");

                break;
        }

        base._Ready();
    }

    protected virtual void OnConfigApplied(TConfig config)
    {
    }
}