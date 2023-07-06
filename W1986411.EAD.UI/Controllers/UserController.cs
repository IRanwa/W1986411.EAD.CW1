using Microsoft.AspNetCore.Mvc;
using W1986411.EAD.Model;
using W1986411.EAD.Service;

namespace W1986411.EAD.UI;

/// <summary>
/// User controller.
/// </summary>
/// <seealso cref="ControllerBase" />
[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    /// <summary>
    /// The service
    /// </summary>
    private readonly IUserService service;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </summary>
    /// <param name="service">The service.</param>
    public UserController(
        IUserService service)
    {
        this.service = service;
    }

    /// <summary>
    /// Registers the user asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Return response.</returns>
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterUserAsync(InsertUpdateUserModel model)
    {
        try
        {
            var response = await service.RegisterUserAsync(model);
            return Ok(response);
        }
        catch(Exception ex)
        {
            return BadRequest();
        }
        
    }

    /// <summary>
    /// Logins the user asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Return response.</returns>
    [HttpPost]
    public async Task<IActionResult> LoginUserAsync(LoginUserModel model)
    {
        try
        {
            var response = await service.LoginUserAsync(model);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }
}