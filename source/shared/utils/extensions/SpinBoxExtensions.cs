using System.Globalization;
using System.Reflection;
using Godot;

namespace ProcGenLab.Shared.Utils;

public static class SpinBoxExtensions
{
    public static void ApplyRangeFrom<TConfig>(this SpinBox spinBox, string propName)
    {
        var hint = typeof(TConfig).GetProperty(propName)?.GetCustomAttribute<ExportAttribute>();

        if (hint?.Hint != PropertyHint.Range || string.IsNullOrEmpty(hint.HintString))
            return;

        spinBox.Rounded = false;

        var parts = hint.HintString.Split(',');
        spinBox.MinValue = Parse(parts, 0);
        spinBox.MaxValue = Parse(parts, 1);
        spinBox.Step = parts.Length >= 3 ? Parse(parts, 2) : 1;
    }

    private static double Parse(string[] parts, int i)
    {
        return double.Parse(parts[i].Trim(), CultureInfo.InvariantCulture);
    }
}