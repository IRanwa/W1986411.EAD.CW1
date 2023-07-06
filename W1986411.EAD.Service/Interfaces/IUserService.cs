using W1986411.EAD.Core;
using W1986411.EAD.Model;

namespace W1986411.EAD.Service;

/// <summary>
/// User service.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Registers the user.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns api response.</returns>
    Task<APIResponse> RegisterUserAsync(InsertUpdateUserModel model);

    /// <summary>
    /// Logins the user asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns login response.</returns>
    Task<APIResponse> LoginUserAsync(LoginUserModel model);
}
