using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using W1986411.EAD.Core;
using W1986411.EAD.Data;
using W1986411.EAD.Model;

namespace W1986411.EAD.Service;

/// <summary>
/// Cheat meal service.
/// </summary>
/// <seealso cref="ICheatMealService" />
public class CheatMealService : ICheatMealService
{
    /// <summary>
    /// The unit of work
    /// </summary>
    private readonly IUnitOfWork unitOfWork;

    /// <summary>
    /// The principal
    /// </summary>
    private readonly IPrincipal principal;

    /// <summary>
    /// The mapper
    /// </summary>
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CheatMealService"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="principal">The principal.</param>
    /// <param name="mapper">The mapper.</param>
    public CheatMealService(
        IUnitOfWork unitOfWork,
        IPrincipal principal,
        IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.principal = principal;
        this.mapper = mapper;
    }

    /// <summary>
    /// Inserts the cheat meal plan asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns insert status.</returns>
    public async Task<APIResponse> InsertCheatMealPlanAsync(InsertUpdateCheatMealModel model)
    {
        try
        {
            var cheatMealPlan = mapper.Map<CheatMealPlan>(model);
            cheatMealPlan.UserId = principal.Identity.Name;
            await unitOfWork.GetGenericRepository<CheatMealPlan>().Add(cheatMealPlan);
            foreach (var food in model.Foods)
            {
                var cheatMealPlanFood = mapper.Map<CheatMealPlanFood>(food);
                cheatMealPlanFood.PlanId = cheatMealPlan.Id;
                await unitOfWork.GetGenericRepository<CheatMealPlanFood>().Add(cheatMealPlanFood);
            }
            unitOfWork.SaveChanges();
            return new APIResponse() { IsSuccess = true, Message = FitnessTrackingRes.Message_CheatMealPlanCreatedSuccess};
        }
        catch (Exception ex)
        {
            ex.GetAllMessages();
        }
        return new APIResponse() { IsSuccess = false, Message = FitnessTrackingRes.Message_CheatMealPlanCreatedFailed };
    }

    /// <summary>
    /// Updates the cheat meal plan asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns update status.</returns>
    public async Task<APIResponse> UpdateCheatMealPlanAsync(InsertUpdateCheatMealModel model)
    {
        try
        {
            var cheatMealPlan = mapper.Map<CheatMealPlan>(model);
            unitOfWork.GetGenericRepository<CheatMealPlan>().Update(cheatMealPlan);
            var foods = unitOfWork.GetGenericRepository<CheatMealPlanFood>()
                .GetQueryable(food => food.PlanId == model.Id && 
                food.CheatMealPlan.UserId == principal.Identity.Name, null).ToList();
            foreach (var food in foods)
                food.IsActive = false;
            foreach (var food in model.Foods)
            {
                var cheatMealPlanFood = mapper.Map<CheatMealPlanFood>(food);
                cheatMealPlanFood.PlanId = cheatMealPlan.Id;
                await unitOfWork.GetGenericRepository<CheatMealPlanFood>().Add(cheatMealPlanFood);
            }
            unitOfWork.SaveChanges();
            return new APIResponse() { IsSuccess = true, Message = FitnessTrackingRes.Message_CheatMealPlanUpdatedSuccess };
        }
        catch (Exception ex)
        {
            ex.GetAllMessages();
        }
        return new APIResponse() { IsSuccess = false, Message = FitnessTrackingRes.Message_CheatMealPlanUpdatedFailed };
    }

    /// <summary>
    /// Gets the cheat meal plans.
    /// </summary>
    /// <returns>Returns list of cheat meal plans.</returns>
    public APIResponse GetCheatMealPlans()
    {
        var cheatMealPlans = unitOfWork.GetGenericRepository<CheatMealPlan>()
            .GetQueryable(plan => plan.IsActive && plan.UserId == principal.Identity.Name, null)
            .Include(plan => plan.CheatMealPlanFoods)
            .ToList();
        foreach (var plan in cheatMealPlans)
            plan.CheatMealPlanFoods = plan.CheatMealPlanFoods.Where(food => food.IsActive).ToList();
        return new APIResponse() { IsSuccess = true, Data = mapper.Map<List<ViewCheatMealPlanModel>>(cheatMealPlans) };
    }

    /// <summary>
    /// Gets the cheat meal plan asynchronous.
    /// </summary>
    /// <param name="planId">The plan identifier.</param>
    /// <returns>Returns cheat meal plan.</returns>
    public APIResponse GetCheatMealPlanAsync(int planId)
    {
        var cheatMealPlan = unitOfWork.GetGenericRepository<CheatMealPlan>()
            .GetQueryable(plan => plan.Id == planId && plan.UserId == principal.Identity.Name, null)
            .Include(plan => plan.CheatMealPlanFoods)
            .FirstOrDefault();
        if (cheatMealPlan == null)
            return new APIResponse() { IsSuccess = false, Message = FitnessTrackingRes.Message_CheatMealPlanRetrieveFailed };
        cheatMealPlan.CheatMealPlanFoods = cheatMealPlan.CheatMealPlanFoods.Where(food => food.IsActive).ToList();
        return new APIResponse() { IsSuccess = true, Data = mapper.Map<CheatMealPlanModel>(cheatMealPlan) };
    }

    /// <summary>
    /// Removes the cheat meal plan asynchronous.
    /// </summary>
    /// <param name="planId">The plan identifier.</param>
    /// <returns>Returns remove status.</returns>
    public APIResponse RemoveCheatMealPlanAsync(int planId)
    {
        var cheatMealPlan = unitOfWork.GetGenericRepository<CheatMealPlan>()
            .GetQueryable(plan => plan.Id == planId && plan.UserId == principal.Identity.Name, null)
            .Include(plan => plan.CheatMealPlanFoods)
            .FirstOrDefault();
        if (cheatMealPlan == null)
            return new APIResponse() { IsSuccess = false, Message = FitnessTrackingRes.Message_CheatMealPlanRemoveFailed };

        foreach (var food in cheatMealPlan.CheatMealPlanFoods)
            food.IsActive = false;
        cheatMealPlan.IsActive = false;
        unitOfWork.SaveChanges();
        return new APIResponse() { IsSuccess = true, Message = FitnessTrackingRes.Message_CheatMealPlanRemoveSuccess };
    }
}
