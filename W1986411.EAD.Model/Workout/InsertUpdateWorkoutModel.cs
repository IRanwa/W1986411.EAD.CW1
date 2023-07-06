using W1986411.EAD.Core;

namespace W1986411.EAD.Model;

/// <summary>
/// Insert update workout model.
/// </summary>
public class InsertUpdateWorkoutModel
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public int Id { get; set; }

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
    /// Gets or sets the routines.
    /// </summary>
    public List<InsertUpdateWorkoutRoutineModel> Routines { get; set; }
}
