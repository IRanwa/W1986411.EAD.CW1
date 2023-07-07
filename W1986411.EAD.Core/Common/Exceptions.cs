using Serilog;

namespace W1986411.EAD.Core;

/// <summary>
/// Exceptions.
/// </summary>
public static class Exceptions
{
    /// <summary>
    /// Gets all messages.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <returns>Returns exceptions.</returns>
    public static string GetAllMessages(this Exception exception)
    {
        var messages = exception.FromHierarchy(ex => ex.InnerException)
            .Select(ex => ex.Message);
        SaveLog(messages);
        return string.Join(Environment.NewLine, messages);
    }

    /// <summary>
    /// Froms the hierarchy.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="nextItem">The next item.</param>
    /// <returns></returns>
    public static IEnumerable<TSource> FromHierarchy<TSource>(
        this TSource source,
        Func<TSource, TSource> nextItem)
        where TSource : class
    {
        return FromHierarchy(source, nextItem, s => s != null);
    }

    /// <summary>
    /// Froms the hierarchy.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="nextItem">The next item.</param>
    /// <param name="canContinue">The can continue.</param>
    /// <returns></returns>
    public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem,
        Func<TSource, bool> canContinue)
    {
        for (var current = source; canContinue(current); current = nextItem(current))
            yield return current;
    }

    /// <summary>
    /// Saves the log.
    /// </summary>
    /// <param name="messages">The messages.</param>
    public static void SaveLog(IEnumerable<string> messages)
    {
        if (messages != null)
        {
            foreach (var message in messages)
            {
                Log.Error(message);
            }
        }
    }
}
