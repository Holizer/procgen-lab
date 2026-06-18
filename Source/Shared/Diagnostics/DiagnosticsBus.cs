using System;

namespace ProcGenLab.Shared.Diagnostics;

public static class DiagnosticsBus
{
    public static event Action<string, double, double, int> BenchmarkUpdated;

    public static void Emit(string algo, double timeMs, double memoryMb, int seed)
    {
        BenchmarkUpdated?.Invoke(algo, timeMs, memoryMb, seed);
    }
}