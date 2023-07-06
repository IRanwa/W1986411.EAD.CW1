using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using W1986411.EAD.Core;

namespace W1986411.EAD.Data;

/// <summary>
/// Workout plan.
/// </summary>
/// <seealso cref="EntityBase" />
public class WorkoutPlan : EntityBase
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
    /// Gets or sets the name of the workout.
    /// </summary>
    public string WorkoutName { get; set; }

    /// <summary>
    /// Gets or sets the type of the occurrence.
    /// </summary>
    public OccurrenceTypes OccurrenceType { get; set; }

    /// <summary>
    /// Gets or sets the start date.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date.
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the workout plan routines.
    /// </summary>
    public virtual ICollection<WorkoutPlanRoutine> WorkoutPlanRoutines { get; set; }

    /// <summary>
    /// Gets or sets the application user.
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser ApplicationUser { get; set; }
}
