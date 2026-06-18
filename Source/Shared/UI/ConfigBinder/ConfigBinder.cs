using System;
using System.Reflection;
using Godot;
using ProcGenLab.Shared.Core;
using ProcGenLab.Shared.Interfaces;
using ProcGenLab.Shared.Utils;

namespace ProcGenLab.Shared.UI;

public class ConfigBinder<TConfig>(Control panel)
    where TConfig : GenerationConfig
{
    private static PropertyInfo[] ConfigProperties => ConfigPropertiesCache<TConfig>.Properties;
    protected TConfig CurrentConfig { get; private set; }

    public virtual void LoadConfig(GenerationConfig defaults)
    {
        CurrentConfig =
            defaults as TConfig
            ?? throw new LabException(
                this,
                $"Expected {typeof(TConfig).Name}, but got {defaults?.GetType().Name ?? "null"}."
            );

        ResetToDefaults(CurrentConfig);

        var randomSeedRow = FindConfigRow(nameof(ISeedableConfig.UseRandomSeed));
        if (randomSeedRow?.CheckBoxNode != null)
        {
            if (
                randomSeedRow.CheckBoxNode.IsConnected(
                    BaseButton.SignalName.Toggled,
                    Callable.From<bool>(OnRandomSeedToggled)
                )
            )
                randomSeedRow.CheckBoxNode.Toggled -= OnRandomSeedToggled;

            randomSeedRow.CheckBoxNode.Toggled += OnRandomSeedToggled;
        }

        OnRandomSeedToggled(((ISeedableConfig)defaults)?.UseRandomSeed ?? false);
    }

    public virtual TConfig BuildConfig()
    {
        if (CurrentConfig == null)
            throw new LabException(this, $"Panel {panel.GetType().Name} is not initialized.");

        var config = CurrentConfig.Clone<TConfig>();
        ApplyRowsToConfig(config);

        if (config is ISeedableConfig seedable && seedable.UseRandomSeed)
        {
            seedable.Seed = GD.RandRange(0, int.MaxValue);
            var seedRow = FindConfigRow(nameof(ISeedableConfig.Seed));
            if (seedRow != null)
                seedRow.Value = seedable.Seed;
        }

        CurrentConfig = config;
        return config;
    }

    private void ResetToDefaults(TConfig defaults)
    {
        if (defaults == null)
            return;

        foreach (var prop in ConfigProperties)
        {
            var row = FindConfigRow(prop.Name);
            if (row == null)
                continue;

            var value = prop.GetValue(defaults);
            if (value is bool b)
                row.IsChecked = b;
            else
                row.Value = Convert.ToDouble(value);

            ApplyRangeIfPresent(prop, row);
        }
    }

    private void ApplyRowsToConfig(TConfig config)
    {
        if (config == null)
            return;

        foreach (var prop in ConfigProperties)
        {
            if (!prop.CanWrite)
                continue;

            var row = FindConfigRow(prop.Name);
            if (row == null)
                continue;

            if (TryConvertValue(prop, row, out var value))
                prop.SetValue(config, value);
        }
    }

    private void ApplyRangeIfPresent(PropertyInfo prop, ConfigRow row)
    {
        var exportAttr = prop.GetCustomAttribute<ExportAttribute>();
        if (exportAttr == null || exportAttr.Hint != PropertyHint.Range)
            return;

        if (row.SpinBoxNode != null)
            row.SpinBoxNode.ApplyRangeFrom<TConfig>(prop.Name);
    }

    private static bool TryConvertValue(PropertyInfo prop, ConfigRow row, out object value)
    {
        var targetType = prop.PropertyType;
        try
        {
            value =
                targetType == typeof(bool)
                    ? row.IsChecked
                    : Convert.ChangeType(row.Value, targetType);
            return true;
        }
        catch
        {
            value = null;
            return false;
        }
    }

    protected ConfigRow FindConfigRow(string name)
    {
        foreach (var child in panel.GetChildren())
        {
            var found = FindConfigRowInNode(child, name);
            if (found != null)
                return found;
        }

        return null;
    }

    private static ConfigRow FindConfigRowInNode(Node node, string name)
    {
        if (
            node is ConfigRow row
            && string.Equals(node.Name, name, StringComparison.OrdinalIgnoreCase)
        )
            return row;

        foreach (var child in node.GetChildren())
        {
            var found = FindConfigRowInNode(child, name);
            if (found != null)
                return found;
        }

        return null;
    }

    private void OnRandomSeedToggled(bool enabled)
    {
        var manualSeedRow = FindConfigRow(nameof(ISeedableConfig.Seed));
        if (manualSeedRow != null)
            manualSeedRow.Visible = !enabled;
    }
}