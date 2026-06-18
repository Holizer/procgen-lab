using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ProcGenLab.Shared.Diagnostics;

public static class BenchmarkService
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BenchmarkResult<TResult> Profile<TState, TResult>(
        Func<TState, TResult> generationFunc,
        TState state
    )
    {
        PrepareGarbageCollection();

        var startAllocatedBytes = GC.GetAllocatedBytesForCurrentThread();
        var stopwatch = Stopwatch.StartNew();

        var result = generationFunc(state);

        stopwatch.Stop();
        var endAllocatedBytes = GC.GetAllocatedBytesForCurrentThread();

        var totalTimeMs = stopwatch.Elapsed.TotalMilliseconds;
        var allocatedMb =
            (double)(endAllocatedBytes - startAllocatedBytes) / (1024 * 1024);

        return new BenchmarkResult<TResult>(totalTimeMs, Math.Max(0.0, allocatedMb), result);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void PrepareGarbageCollection()
    {
        GC.Collect(2, GCCollectionMode.Forced, true, true);
        GC.WaitForPendingFinalizers();
        GC.Collect(2, GCCollectionMode.Forced, true, true);
    }
}

public readonly record struct BenchmarkResult<T>(double TimeMs, double MemoryMb, T Result);