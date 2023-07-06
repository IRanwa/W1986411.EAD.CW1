using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace W1986411.EAD.Data;

/// <summary>
/// Cheat meal plan food.
/// </summary>
/// <seealso cref="EntityBase" />
public class CheatMealPlanFood : EntityBase
{
    /// <summary>
    /// Gets or sets the food identifier.
    /// </summary>
    [Key]
    public int FoodId { get; set; }

    /// <summary>
    /// Gets or sets the plan identifier.
    /// </summary>
    public int PlanId { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the calories gain.
    /// </summary>
    public double CaloriesGain { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the cheat meal plan.
    /// </summary>
    [JsonIgnore]
    [ForeignKey(nameof(PlanId))]
    public virtual CheatMealPlan CheatMealPlan { get; set; }
}
