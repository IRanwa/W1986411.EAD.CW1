using System.ComponentModel.DataAnnotations;

namespace W1986411.EAD.Core;

/// <summary>
/// Occurrence type.
/// </summary>
public enum OccurrenceTypes
{
    /// <summary>
    /// The one time
    /// </summary>
    [Display(Name ="One Time")]
    OneTime = 1,

    /// <summary>
    /// The recurring
    /// </summary>
    [Display(Name = "Recurring")]
    Recurring = 2
}
