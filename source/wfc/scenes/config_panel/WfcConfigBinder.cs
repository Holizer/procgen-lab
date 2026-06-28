using System.Collections.Generic;
using System.Linq;
using Godot;
using ProcGenLab.Shared.Core;
using ProcGenLab.Shared.UI;
using ProcGenLab.WFC.Enums;
using ProcGenLab.WFC.Resources;

namespace ProcGenLab.WFC.Scenes;

public class WfcConfigBinder(WfcConfigPanel panel) : ConfigBinder<WfcConfig>(panel)
{
    private readonly Dictionary<MacroTileType, ConfigRow> _weightConfigRows = [];

    public override void LoadConfig(GenerationConfig defaults)
    {
        base.LoadConfig(defaults);
        BuildWeightsUi(CurrentConfig.Weights);

        var topologyRow = FindConfigRow(nameof(WfcConfig.UseBspTopology));

        if (topologyRow?.CheckBoxNode != null)
        {
            if (
                topologyRow.CheckBoxNode.IsConnected(
                    BaseButton.SignalName.Toggled,
                    Callable.From<bool>(panel.OnTopologyToggled)
                )
            )
                topologyRow.CheckBoxNode.Toggled -= panel.OnTopologyToggled;

            topologyRow.CheckBoxNode.Toggled += panel.OnTopologyToggled;
        }

        panel.OnTopologyToggled(CurrentConfig.UseBspTopology);
    }

    public override WfcConfig BuildConfig()
    {
        var config = base.BuildConfig();
        config.Weights = BuildWeightsConfig();

        return config;
    }

    private void BuildWeightsUi(WfcWeightConfig weights)
    {
        panel.WeightsRowsContainer.Clear();
        _weightConfigRows.Clear();

        if (weights?.Weights is null)
            return;

        foreach (var entry in weights.Weights)
        {
            var row = panel.ConfigRowScene.Instantiate<ConfigRow>();
            row.Setup(entry.Type.ToString(), entry.Weight, "%");
            panel.WeightsRowsContainer.AddItem(row);
            _weightConfigRows[entry.Type] = row;
        }
    }

    private WfcWeightConfig BuildWeightsConfig()
    {
        return new WfcWeightConfig
        {
            Weights = _weightConfigRows
                .Select(kv => new WfcWeightEntry { Type = kv.Key, Weight = (float)kv.Value.Value })
                .ToArray()
        };
    }
}