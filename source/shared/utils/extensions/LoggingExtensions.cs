using System.Runtime.CompilerServices;
using Godot;

namespace ProcGenLab.Shared.Utils;

public static class LoggingExtensions
{
    public static void LogError(
        this object obj,
        string message,
        [CallerMemberName] string methodName = ""
    )
    {
        GD.PushError($"[{obj.GetType().Name} ({methodName})]:\n{message}");
    }

    public static void LogWarning(
        this object obj,
        string message,
        [CallerMemberName] string methodName = ""
    )
    {
        GD.PushWarning($"[{obj.GetType().Name} ({methodName})]:\n{message}");
    }

    public static void LogInfo(
        this object obj,
        string message,
        [CallerMemberName] string methodName = ""
    )
    {
        var header = $"[{obj.GetType().Name} ({methodName})]:";
        GD.PrintRich($"[color=green]{header}[/color]\n{message}");
    }

    public static void LogCustom(
        this object obj,
        string message,
        string color,
        [CallerMemberName] string methodName = ""
    )
    {
        var header = $"[{obj.GetType().Name} ({methodName})]:";
        GD.PrintRich($"[color={color}]{header}[/color]\n{message}");
    }
}