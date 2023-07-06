namespace W1986411.EAD.Model;

/// <summary>
/// Insert update workout routine model.
/// </summary>
public class InsertUpdateWorkoutRoutineModel
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public int RoutineId { get; set; }

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
}
