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
            return new APIResponse() { Status = true, Message = FitnessTrackingRes.Message_CheatMealPlanCreatedSuccess};
        }
        catch (Exception ex)
        {

        }
        return new APIResponse() { Status = false, Message = FitnessTrackingRes.Message_CheatMealPlanCreatedFailed };
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
                .GetQueryable(food => food.PlanId == model.Id, null).ToList();
            foreach (var food in model.Foods)
            {
                if (food.FoodId == default)
                {
                    var cheatMealPlanFood = mapper.Map<CheatMealPlanFood>(food);
                    cheatMealPlanFood.PlanId = cheatMealPlan.Id;
                    await unitOfWork.GetGenericRepository<CheatMealPlanFood>().Add(cheatMealPlanFood);
                }
                else
                {
                    var currentFood = foods.FirstOrDefault(cheatMealFood => cheatMealFood.FoodId == food.FoodId);
                    if (currentFood == null)
                        continue;
                    currentFood.CaloriesGain = food.CaloriesGain;
                    currentFood.Name = food.Name;
                    currentFood.IsActive = food.IsActive;
                    unitOfWork.GetGenericRepository<CheatMealPlanFood>().Update(currentFood);
                }
            }
            unitOfWork.SaveChanges();
            return new APIResponse() { Status = true, Message = FitnessTrackingRes.Message_CheatMealPlanUpdatedSuccess };
        }
        catch (Exception ex)
        {

        }
        return new APIResponse() { Status = false, Message = FitnessTrackingRes.Message_CheatMealPlanUpdatedFailed };
    }

    /// <summary>
    /// Gets the cheat meal plans.
    /// </summary>
    /// <returns>Returns list of cheat meal plans.</returns>
    public APIResponse GetCheatMealPlans()
    {
        var cheatMealPlans = unitOfWork.GetGenericRepository<CheatMealPlan>()
            .GetQueryable(plan => plan.IsActive, null)
            .Include(plan => plan.CheatMealPlanFoods)
            .ToList();
        foreach (var plan in cheatMealPlans)
            plan.CheatMealPlanFoods = plan.CheatMealPlanFoods.Where(food => food.IsActive).ToList();
        return new APIResponse() { Status = true, Data = mapper.Map<List<CheatMealPlan>>(cheatMealPlans) };
    }
}
