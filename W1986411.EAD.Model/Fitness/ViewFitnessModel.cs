using W1986411.EAD.Core;

namespace W1986411.EAD.Model;

/// <summary>
/// View fitness model.
/// </summary>
public class ViewFitnessModel
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public int Id { get; set; }

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
    /// Gets or sets the fitness status string.
    /// </summary>
    public string FitnessStatusStr { get; set; }

    /// <summary>
    /// Gets or sets the calories burn.
    /// </summary>
    public double CaloriesBurn { get; set; }

    /// <summary>
    /// Gets or sets the calories gain.
    /// </summary>
    public double CaloriesGain { get; set; }
}
