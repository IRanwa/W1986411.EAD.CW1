using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace W1986411.EAD.Core;

/// <summary>
/// Extensions.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Gets the display name of the enum.
    /// </summary>
    /// <param name="enumValue">The enum value.</param>
    /// <returns>Returns display name.</returns>
    public static string GetEnumDisplayName(this Enum enumValue)
    {
        return enumValue.GetType()
                        .GetMember(enumValue.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>().Name;
    }
}
