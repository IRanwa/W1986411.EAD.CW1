using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W1986411.EAD.Core;
using W1986411.EAD.Model;
using W1986411.EAD.Service;

namespace W1986411.EAD.UI;

/// <summary>
/// Workout controller.
/// </summary>
/// <seealso cref="ControllerBase" />
[Route("api/v1/workout")]
[ApiController]
[Authorize]
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
            ex.GetAllMessages();
            return BadRequest();
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
            ex.GetAllMessages();
            return BadRequest();
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
            ex.GetAllMessages();
            return BadRequest();
        }
    }

    /// <summary>
    /// Removes the workout plan asynchronous.
    /// </summary>
    /// <param name="planId">The plan identifier.</param>
    /// <returns>Returns response.</returns>
    [HttpPost]
    [Route("remove/{planId}")]
    public IActionResult RemoveWorkoutPlanAsync(int planId)
    {
        try
        {
            var response = service.RemoveWorkoutPlanAsync(planId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            ex.GetAllMessages();
            return BadRequest();
        }
    }

    /// <summary>
    /// Gets the workout plan asynchronous.
    /// </summary>
    /// <param name="planId">The plan identifier.</param>
    /// <returns>Returns response.</returns>
    [HttpGet]
    [Route("{planId}")]
    public IActionResult GetWorkoutPlan(int planId)
    {
        try
        {
            var response = service.GetWorkoutPlanAsync(planId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            ex.GetAllMessages();
            return BadRequest();
        }
    }
}