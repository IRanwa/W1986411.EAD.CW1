using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using W1986411.EAD.Core;
using W1986411.EAD.Data;
using W1986411.EAD.Model;

namespace W1986411.EAD.Service;

/// <summary>
/// Workout service.
/// </summary>
/// <seealso cref="IWorkoutService" />
public class WorkoutService : IWorkoutService
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
    /// Initializes a new instance of the <see cref="WorkoutService"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="principal">The principal.</param>
    /// <param name="mapper">The mapper.</param>
    public WorkoutService(
        IUnitOfWork unitOfWork,
        IPrincipal principal,
        IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.principal = principal;
        this.mapper = mapper;
    }

    /// <summary>
    /// Inserts the workout plan asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns insert status.</returns>
    public async Task<APIResponse> InsertWorkoutPlanAsync(InsertUpdateWorkoutModel model)
    {
        try
        {
            var workoutPlan = mapper.Map<WorkoutPlan>(model);
            workoutPlan.UserId = principal.Identity.Name;
            await unitOfWork.GetGenericRepository<WorkoutPlan>().Add(workoutPlan);
            foreach (var routine in model.Routines)
            {
                var workoutRoutine = mapper.Map<WorkoutPlanRoutine>(routine);
                workoutRoutine.PlanId = workoutPlan.Id;
                await unitOfWork.GetGenericRepository<WorkoutPlanRoutine>().Add(workoutRoutine);
            }
            unitOfWork.SaveChanges();
            return new APIResponse() { IsSuccess = true, Message = FitnessTrackingRes.Message_WorkoutPlanCreatedSuccess };
        }
        catch (Exception ex)
        {
            ex.GetAllMessages();
        }
        return new APIResponse() { IsSuccess = false, Message = FitnessTrackingRes.Message_WorkoutPlanCreatedFailed };
    }

    /// <summary>
    /// Updates the workout plan asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns update status.</returns>
    public async Task<APIResponse> UpdateWorkoutPlanAsync(InsertUpdateWorkoutModel model)
    {
        try
        {
            var workoutPlan = mapper.Map<WorkoutPlan>(model);
            unitOfWork.GetGenericRepository<WorkoutPlan>().Update(workoutPlan);
            var routines = unitOfWork.GetGenericRepository<WorkoutPlanRoutine>()
                .GetQueryable(routine => routine.PlanId == model.Id &&
                    routine.WorkoutPlan.UserId == principal.Identity.Name, null).ToList();
            foreach (var routine in routines)
                routine.IsActive = false;
            foreach (var routine in model.Routines)
            {
                var workoutRoutine = mapper.Map<WorkoutPlanRoutine>(routine);
                workoutRoutine.PlanId = workoutPlan.Id;
                await unitOfWork.GetGenericRepository<WorkoutPlanRoutine>().Add(workoutRoutine);
            }
            unitOfWork.SaveChanges();
            return new APIResponse() { IsSuccess = true, Message = FitnessTrackingRes.Message_WorkoutPlanUpdatedSuccess };
        }
        catch (Exception ex)
        {
            ex.GetAllMessages();
        }
        return new APIResponse() { IsSuccess = false, Message = FitnessTrackingRes.Message_WorkoutPlanUpdatedFailed };
    }

    /// <summary>
    /// Gets the workout plans.
    /// </summary>
    /// <returns>Return workout plans.</returns>
    public APIResponse GetWorkoutPlans()
    {
        var workoutPlans = unitOfWork.GetGenericRepository<WorkoutPlan>()
            .GetQueryable(plan => plan.IsActive &&
                    plan.UserId == principal.Identity.Name, null)
            .Include(plan => plan.WorkoutPlanRoutines)
            .ToList();
        foreach (var plan in workoutPlans)
            plan.WorkoutPlanRoutines = plan.WorkoutPlanRoutines.Where(routine => routine.IsActive).ToList();
        return new APIResponse() { IsSuccess = true, Data = mapper.Map<List<ViewWorkoutPlanModel>>(workoutPlans) };
    }

    /// <summary>
    /// Gets the workout plan asynchronous.
    /// </summary>
    /// <param name="planId">The plan identifier.</param>
    /// <returns>Returns specific workout.</returns>
    public APIResponse GetWorkoutPlanAsync(int planId)
    {
        var workoutPlan = unitOfWork.GetGenericRepository<WorkoutPlan>()
            .GetQueryable(plan => plan.Id == planId &&
                    plan.UserId == principal.Identity.Name, null)
            .Include(plan => plan.WorkoutPlanRoutines)
            .FirstOrDefault();
        if (workoutPlan == null)
            return new APIResponse() { IsSuccess = false, Message = FitnessTrackingRes.Message_WorkoutPlanRetrieveFailed };
        workoutPlan.WorkoutPlanRoutines = workoutPlan.WorkoutPlanRoutines.Where(routine => routine.IsActive).ToList();
        return new APIResponse() { IsSuccess = true, Data = mapper.Map<WorkoutPlanModel>(workoutPlan) };
    }

    /// <summary>
    /// Removes the workout plan asynchronous.
    /// </summary>
    /// <param name="planId">The plan identifier.</param>
    /// <returns>Returns delete status.</returns>
    public APIResponse RemoveWorkoutPlanAsync(int planId)
    {
        var workoutPlan = unitOfWork.GetGenericRepository<WorkoutPlan>()
            .GetQueryable(plan => plan.Id == planId &&
                    plan.UserId == principal.Identity.Name, null)
            .Include(plan => plan.WorkoutPlanRoutines)
            .FirstOrDefault();
        if (workoutPlan == null)
            return new APIResponse() { IsSuccess = false, Message = FitnessTrackingRes.Message_WorkoutPlanRemoveFailed };

        foreach (var routine in workoutPlan.WorkoutPlanRoutines)
            routine.IsActive = false;
        workoutPlan.IsActive = false;
        unitOfWork.SaveChanges();
        return new APIResponse() { IsSuccess = true, Message = FitnessTrackingRes.Message_WorkoutPlanRemoveSuccess };
    }
}
