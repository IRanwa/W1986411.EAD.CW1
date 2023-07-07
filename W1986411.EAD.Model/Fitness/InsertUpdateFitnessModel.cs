using W1986411.EAD.Core;

namespace W1986411.EAD.Model;

/// <summary>
/// Insert update weight model.
/// </summary>
public class InsertUpdateFitnessModel
{
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
}
