using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using W1986411.EAD.Core;

namespace W1986411.EAD.Data;

/// <summary>
/// User fitness detail.
/// </summary>
/// <seealso cref="EntityBase" />
public class UserFitnessDetail : EntityBase
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the weight.
    /// </summary>
    public double Weight { get; set; }

    /// <summary>
    /// Gets or sets the fitness status.
    /// </summary>
    public FitnessStatus FitnessStatus { get; set; }

    /// <summary>
    /// Gets or sets the record date.
    /// </summary>
    public DateTime RecordDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the application user.
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser ApplicationUser { get; set; }


}
