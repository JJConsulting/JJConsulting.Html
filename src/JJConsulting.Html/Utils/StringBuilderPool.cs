using System;
using System.Text;

namespace JJConsulting.Html.Utils;

/// <summary>
/// A thread-static pool for reusing StringBuilder instances.
/// Converted from F# to C# from https://github.com/giraffe-fsharp/Giraffe.ViewEngine/blob/b012e4eeb02f9e67289a5432e122643a51587feb/src/Giraffe.ViewEngine/StringBuilderPool.fs
/// </summary>
internal static class StringBuilderPool
{
    [ThreadStatic]
    private static StringBuilder? _instance;

    [ThreadStatic]
    private static DateTimeOffset _created;

    private const int MinimumCapacity = 5000;
    private const int MaximumCapacity = 40000;
    private static readonly TimeSpan MaximumLifetime = TimeSpan.FromMinutes(10.0);

    [field: ThreadStatic]
    public static bool IsEnabled { get; set; }

    public static StringBuilder Rent()
    {
        if (!IsEnabled)
        {
            return new StringBuilder(MinimumCapacity);
        }

        var lifetime = DateTimeOffset.Now - _created;
        var expired = lifetime > MaximumLifetime;

        if (!expired && _instance != null)
        {
            var sb = _instance;
            _instance = null;
            sb.Clear();
            return sb;
        }

        return new StringBuilder(MinimumCapacity);
    }

    public static void Release(StringBuilder sb)
    {
        if (sb.Capacity <= MaximumCapacity)
        {
            _instance = sb;
            _created = DateTimeOffset.Now;
        }
    }
}