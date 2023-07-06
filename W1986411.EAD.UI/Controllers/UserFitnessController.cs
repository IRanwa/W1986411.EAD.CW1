using Microsoft.AspNetCore.Mvc;
using W1986411.EAD.Model;
using W1986411.EAD.Service;

namespace W1986411.EAD.UI;

/// <summary>
/// User fitness controller.
/// </summary>
/// <seealso cref="ControllerBase" />
[Route("api/v1/user-fitness")]
[ApiController]
public class UserFitnessController : ControllerBase
{
    /// <summary>
    /// The service
    /// </summary>
    private readonly IUserFitnessService service;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserFitnessController"/> class.
    /// </summary>
    /// <param name="service">The service.</param>
    public UserFitnessController(IUserFitnessService service)
    {
        this.service = service;
    }

    /// <summary>
    /// Gets the user fitness detail by date asynchronous.
    /// </summary>
    /// <param name="date">The date.</param>
    /// <returns>Returns response.</returns>
    [HttpPost]
    [Route("by-date")]
    public async Task<IActionResult> GetUserFitnessDetailByDateAsync(FitnessDetailsFilterModel model)
    {
        try
        {
            var response = await service.GetUserFitnessDataByDateAsync(model);
            return Ok(response);
        }catch(Exception ex)
        {
            return BadRequest();
        }
    }

    /// <summary>
    /// Gets the fitness details for period asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns response.</returns>
    [HttpPost]
    [Route("by-period")]
    public IActionResult GetFitnessDetailsForPeriodAsync(FitnessDetailsFilterModel model)
    {
        try
        {
            var response = service.GetFitnessDetailsForPeriodAsync(model);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    /// <summary>
    /// Inserts the update fitness status.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns response.</returns>
    [HttpPost]
    public async Task<IActionResult> InsertUpdateFitnessStatus(InsertUpdateFitnessModel model)
    {
        try
        {
            var response = await service.InsertUpdateWeightAsync(model);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }
}
