using System;
using System.Runtime.CompilerServices;

namespace ProcGenLab.Shared.Utils;

public class LabException(object caller, string message, [CallerMemberName] string methodName = "")
    : Exception($"[{caller.GetType().Name} ({methodName})]:\n{message}");