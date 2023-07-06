using W1986411.EAD.Core;

namespace W1986411.EAD.Model;

/// <summary>
/// Cheat meal plan model.
/// </summary>
public class CheatMealPlanModel
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the cheat meal.
    /// </summary>
    public string CheatMealName { get; set; }

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
    /// Gets or sets the cheat meal plan foods.
    /// </summary>
    public List<CheatMealPlanFoodModel> CheatMealPlanFoods { get; set; }
}
