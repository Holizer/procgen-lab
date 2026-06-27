using System.Reflection;

namespace ProcGenLab.Shared.UI;

public static class ConfigPropertiesCache<T>
{
    public static readonly PropertyInfo[] Properties = typeof(T).GetProperties(
        BindingFlags.Public | BindingFlags.Instance
    );
}