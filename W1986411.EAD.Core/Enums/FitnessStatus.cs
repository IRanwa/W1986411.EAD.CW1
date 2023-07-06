using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace W1986411.EAD.Core;

/// <summary>
/// Fitness status.
/// </summary>
public enum FitnessStatus
{
    /// <summary>
    /// The very poor
    /// </summary>
    [Display(Name = "Very Poor")]
    VeryPoor = 1,

    /// <summary>
    /// The poor
    /// </summary>
    [Display(Name = "Poor")]
    Poor = 2,

    /// <summary>
    /// The fair
    /// </summary>
    [Display(Name = "Fair")]
    Fair = 3,

    /// <summary>
    /// The good
    /// </summary>
    [Display(Name = "Good")]
    Good = 4,

    /// <summary>
    /// The excellent
    /// </summary>
    [Display(Name = "Excellent")]
    Excellent = 5,

    /// <summary>
    /// The superior
    /// </summary>
    [Display(Name = "Superior")]
    Superior = 6
}
