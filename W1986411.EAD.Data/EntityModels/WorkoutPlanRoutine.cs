using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace W1986411.EAD.Data;

/// <summary>
/// Workout plan routine.
/// </summary>
/// <seealso cref="EntityBase" />
public class WorkoutPlanRoutine : EntityBase
{
    /// <summary>
    /// Gets or sets the routine identifier.
    /// </summary>
    [Key]
    public int RoutineId { get; set; }

    /// <summary>
    /// Gets or sets the plan identifier.
    /// </summary>
    public int PlanId { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the sets.
    /// </summary>
    public int Sets { get; set; }

    /// <summary>
    /// Gets or sets the reps.
    /// </summary>
    public int Reps { get; set; }

    /// <summary>
    /// Gets or sets the burn calories.
    /// </summary>
    public double BurnCalories { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the workout plan.
    /// </summary>
    [JsonIgnore]
    [ForeignKey(nameof(PlanId))]
    public virtual WorkoutPlan WorkoutPlan { get; set; }
}
