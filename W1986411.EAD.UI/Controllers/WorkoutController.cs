using Microsoft.AspNetCore.Mvc;
using W1986411.EAD.Model;
using W1986411.EAD.Service;

namespace W1986411.EAD.UI;

/// <summary>
/// Workout controller.
/// </summary>
/// <seealso cref="ControllerBase" />
[Route("api/workout")]
[ApiController]
public class WorkoutController : ControllerBase
{
    /// <summary>
    /// The service
    /// </summary>
    private readonly IWorkoutService service;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkoutController"/> class.
    /// </summary>
    /// <param name="service">The service.</param>
    public WorkoutController(
        IWorkoutService service)
    {
        this.service = service;
    }

    /// <summary>
    /// Gets the workout plans.
    /// </summary>
    /// <returns>Returns response.</returns>
    [HttpGet]
    public IActionResult GetWorkoutPlans()
    {
        try
        {
            var response = service.GetWorkoutPlans();
            return Ok(response);
        }catch(Exception ex)
        {
            return BadRequest(ex);
        }
    }

    /// <summary>
    /// Inserts the workout plans asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns response.</returns>
    [HttpPost]
    public async Task<IActionResult> InsertWorkoutPlansAsync(InsertUpdateWorkoutModel model)
    {
        try
        {
            var response = await service.InsertWorkoutPlanAsync(model);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    /// <summary>
    /// Updates the workout plans asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns response.</returns>
    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> UpdateWorkoutPlansAsync(InsertUpdateWorkoutModel model)
    {
        try
        {
            var response = await service.UpdateWorkoutPlanAsync(model);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}