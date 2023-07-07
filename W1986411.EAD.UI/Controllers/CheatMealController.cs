using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W1986411.EAD.Core;
using W1986411.EAD.Model;
using W1986411.EAD.Service;

namespace W1986411.EAD.UI;

/// <summary>
/// Cheat meal controller.
/// </summary>
/// <seealso cref="ControllerBase" />
[Route("api/v1/cheatmeal")]
[ApiController]
[Authorize]
public class CheatMealController : ControllerBase
{
    /// <summary>
    /// The service
    /// </summary>
    private readonly ICheatMealService service;

    /// <summary>
    /// Initializes a new instance of the <see cref="CheatMealController"/> class.
    /// </summary>
    /// <param name="service">The service.</param>
    public CheatMealController(
        ICheatMealService service)
    {
        this.service = service;
    }

    /// <summary>
    /// Gets the cheat meal plans.
    /// </summary>
    /// <returns>Returns response.</returns>
    [HttpGet]
    public IActionResult GetCheatMealPlans()
    {
        try
        {
            var response = service.GetCheatMealPlans();
            return Ok(response);
        }
        catch (Exception ex)
        {
            ex.GetAllMessages();
            return BadRequest();
        }
    }

    /// <summary>
    /// Inserts the cheat plans asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns response.</returns>
    [HttpPost]
    public async Task<IActionResult> InsertCheatPlansAsync(InsertUpdateCheatMealModel model)
    {
        try
        {
            var response = await service.InsertCheatMealPlanAsync(model);
            return Ok(response);
        }
        catch (Exception ex)
        {
            ex.GetAllMessages();
            return BadRequest();
        }
    }

    /// <summary>
    /// Updates the cheat meal plans asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns response.</returns>
    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> UpdateCheatMealPlansAsync(InsertUpdateCheatMealModel model)
    {
        try
        {
            var response = await service.UpdateCheatMealPlanAsync(model);
            return Ok(response);
        }
        catch (Exception ex)
        {
            ex.GetAllMessages();
            return BadRequest();
        }
    }

    /// <summary>
    /// Removes the cheat meal plan asynchronous.
    /// </summary>
    /// <param name="planId">The plan identifier.</param>
    /// <returns>Return response.</returns>
    [HttpPost]
    [Route("remove/{planId}")]
    public IActionResult RemoveCheatMealPlanAsync(int planId)
    {
        try
        {
            var response = service.RemoveCheatMealPlanAsync(planId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            ex.GetAllMessages();
            return BadRequest();
        }
    }

    /// <summary>
    /// Gets the cheat meal plan.
    /// </summary>
    /// <param name="planId">The plan identifier.</param>
    /// <returns>Returns response.</returns>
    [HttpGet]
    [Route("{planId}")]
    public IActionResult GetCheatMealPlan(int planId)
    {
        try
        {
            var response = service.GetCheatMealPlanAsync(planId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            ex.GetAllMessages();
            return BadRequest();
        }
    }
}