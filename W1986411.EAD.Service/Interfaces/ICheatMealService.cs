using W1986411.EAD.Core;
using W1986411.EAD.Model;

namespace W1986411.EAD.Service;

/// <summary>
/// Cheat meal service.
/// </summary>
public interface ICheatMealService
{
    /// <summary>
    /// Inserts the cheat meal plan asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns insert status.</returns>
    Task<APIResponse> InsertCheatMealPlanAsync(InsertUpdateCheatMealModel model);

    /// <summary>
    /// Updates the cheat meal plan asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns update status.</returns>
    Task<APIResponse> UpdateCheatMealPlanAsync(InsertUpdateCheatMealModel model);

    /// <summary>
    /// Gets the cheat meal plans.
    /// </summary>
    /// <returns>Returns list of cheat meal plans.</returns>
    APIResponse GetCheatMealPlans();

    /// <summary>
    /// Gets the cheat meal plan asynchronous.
    /// </summary>
    /// <param name="planId">The plan identifier.</param>
    /// <returns>Returns cheat meal plan.</returns>
    APIResponse GetCheatMealPlanAsync(int planId);

    /// <summary>
    /// Removes the cheat meal plan asynchronous.
    /// </summary>
    /// <param name="planId">The plan identifier.</param>
    /// <returns>Returns remove status.</returns>
    APIResponse RemoveCheatMealPlanAsync(int planId);
}
