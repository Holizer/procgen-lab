using Godot;
using ProcGenLab.Shared.Diagnostics;

namespace ProcGenLab.Lobby.UI;

public partial class PerformanceMonitor : Control
{
    private readonly Color _colorGold = new("f2be32");

    private readonly Color _colorGreen = new("5cb85c");

    private readonly Color _colorOrange = new("f0ad4e");

    private readonly Color _colorRed = new("d9534f");

    private readonly Color _colorWhite = new("ffffff");

    [Export] public Label AlgoValueLabel { get; set; }

    [Export] public Label TimeValueLabel { get; set; }

    [Export] public Label MemoryValueLabel { get; set; }

    [Export] public Label SeedValueLabel { get; set; }

    public override void _Ready()
    {
        DiagnosticsBus.BenchmarkUpdated += OnBenchmarkUpdated;
        Visible = false;
    }

    public override void _ExitTree()
    {
        DiagnosticsBus.BenchmarkUpdated -= OnBenchmarkUpdated;
    }

    private void OnBenchmarkUpdated(string algo, double timeMs, double memoryMb, int seed)
    {
        Visible = true;

        if (AlgoValueLabel is not null)
            AlgoValueLabel.Text = algo;

        if (SeedValueLabel is not null)
        {
            SeedValueLabel.Text = seed.ToString();
            SeedValueLabel.Modulate = _colorWhite;
        }

        if (TimeValueLabel is not null)
        {
            TimeValueLabel.Text = $"{timeMs:F2} ms";

            if (timeMs < 15.0)
                TimeValueLabel.Modulate = _colorGreen;
            else if (timeMs < 100.0)
                TimeValueLabel.Modulate = _colorOrange;
            else
                TimeValueLabel.Modulate = _colorRed;
        }

        if (MemoryValueLabel is not null)
        {
            MemoryValueLabel.Text = $"{memoryMb:F3} MB";
            MemoryValueLabel.Modulate = memoryMb < 0.100 ? _colorGreen : _colorWhite;
        }
    }
}