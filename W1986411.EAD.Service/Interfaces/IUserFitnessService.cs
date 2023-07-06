using W1986411.EAD.Core;
using W1986411.EAD.Model;

namespace W1986411.EAD.Service;

/// <summary>
/// user fitness service.
/// </summary>
public interface IUserFitnessService
{
    /// <summary>
    /// Inserts the update weight asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns response.</returns>
    Task<APIResponse> InsertUpdateWeightAsync(InsertUpdateFitnessModel model);

    /// <summary>
    /// Gets the user fitness data by date asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns user fitness details for the date.</returns>
    Task<APIResponse> GetUserFitnessDataByDateAsync(FitnessDetailsFilterModel model);

    /// <summary>
    /// Gets the fitness details for period asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns list of records.</returns>
    APIResponse GetFitnessDetailsForPeriodAsync(FitnessDetailsFilterModel model);
}
