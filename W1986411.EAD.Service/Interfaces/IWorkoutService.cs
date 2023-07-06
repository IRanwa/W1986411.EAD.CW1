using W1986411.EAD.Core;
using W1986411.EAD.Model;

namespace W1986411.EAD.Service;

/// <summary>
/// Workout service.
/// </summary>
public interface IWorkoutService
{
    /// <summary>
    /// Inserts the workout plan asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns insert status.</returns>
    Task<APIResponse> InsertWorkoutPlanAsync(InsertUpdateWorkoutModel model);

    /// <summary>
    /// Updates the workout plan asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns update status.</returns>
    Task<APIResponse> UpdateWorkoutPlanAsync(InsertUpdateWorkoutModel model);

    /// <summary>
    /// Gets the workout plans.
    /// </summary>
    /// <returns>Return workout plans.</returns>
    APIResponse GetWorkoutPlans();
}
