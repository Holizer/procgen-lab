using System;
using System.Runtime.CompilerServices;
using Godot;
using ProcGenLab.Shared.Utils;

namespace ProcGenLab.Shared.Core;

public abstract class BaseMapBuilder<TBuilder, TMap, TConfig>
    where TBuilder : BaseMapBuilder<TBuilder, TMap, TConfig>
    where TMap : Grid2D
    where TConfig : GenerationConfig
{
    protected TMap Map { get; set; }
    protected TConfig Config { get; private set; }
    protected RandomNumberGenerator Rng { get; private set; }
    public bool IsFaulted { get; protected set; }
    protected abstract TMap EmptyMap { get; }

    private TBuilder Self => (TBuilder)this;

    protected virtual void Configure(TConfig config)
    {
        Config = config ?? throw new ArgumentNullException(nameof(config));
        Rng = config.CreateRng();
        IsFaulted = false;
        Map = null;
    }

    protected virtual TBuilder Step(Action action, [CallerMemberName] string name = "")
    {
        TryExecute(action, name);
        return Self;
    }

    protected void TryExecute(Action action, string stageName)
    {
        if (IsFaulted)
            return;

        try
        {
            action();
        }
        catch (Exception e)
        {
            this.LogError($"{stageName} failed: {e.Message}");
            IsFaulted = true;
        }
    }

    public virtual TMap Build()
    {
        return IsFaulted || Map == null ? EmptyMap : Map;
    }
}