using W1986411.EAD.Core;

namespace W1986411.EAD.Model;

/// <summary>
/// View workout plan model.
/// </summary>
public class ViewWorkoutPlanModel
{
    /// <summary>
    /// Gets or sets the plan identifier.
    /// </summary>
    public int PlanId { get; set; }

    /// <summary>
    /// Gets or sets the name of the plan.
    /// </summary>
    public string PlanName { get; set; }

    /// <summary>
    /// Gets or sets the type of the occurrence.
    /// </summary>
    public string occurrenceType { get; set; }

    /// <summary>
    /// Gets or sets the calories burn.
    /// </summary>
    public double CaloriesBurn { get; set; }
}
