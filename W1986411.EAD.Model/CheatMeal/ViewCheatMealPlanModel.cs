namespace W1986411.EAD.Model;

/// <summary>
/// View cheat meal plan model.
/// </summary>
public class ViewCheatMealPlanModel
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the type of the occurrence.
    /// </summary>
    public string OccurrenceType { get; set; }

    /// <summary>
    /// Gets or sets the start date.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date.
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Gets or sets the calories gain.
    /// </summary>
    public double CaloriesGain { get; set; }
}
