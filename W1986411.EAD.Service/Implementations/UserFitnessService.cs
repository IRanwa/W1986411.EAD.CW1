using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using W1986411.EAD.Core;
using W1986411.EAD.Data;
using W1986411.EAD.Model;

namespace W1986411.EAD.Service;

/// <summary>
/// User fitness service.
/// </summary>
/// <seealso cref="IUserFitnessService" />
public class UserFitnessService : IUserFitnessService
{
    /// <summary>
    /// The unit of work
    /// </summary>
    private readonly IUnitOfWork unitOfWork;

    /// <summary>
    /// The user
    /// </summary>
    private readonly IPrincipal user;

    /// <summary>
    /// The mapper
    /// </summary>
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserFitnessService"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="user">The user.</param>
    /// <param name="mapper">The mapper.</param>
    public UserFitnessService(
        IUnitOfWork unitOfWork, 
        IPrincipal user,
        IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.user = user;
        this.mapper = mapper;
    }

    /// <summary>
    /// Inserts the update weight asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns response.</returns>
    public async Task<APIResponse> InsertUpdateWeightAsync(InsertUpdateFitnessModel model)
    {
        var fitnessRecord = await unitOfWork.GetGenericRepository<UserFitnessDetail>().GetOneAsync(x => 
            x.RecordDate.Year == model.RecordDate.Year &&
            x.RecordDate.Month == model.RecordDate.Month &&
            x.RecordDate.Date == model.RecordDate.Date );
        if (fitnessRecord == null)
        {
            await unitOfWork.GetGenericRepository<UserFitnessDetail>().Add(new UserFitnessDetail()
            {
                RecordDate = model.RecordDate,
                UserId = user.Identity.Name,
                Weight = model.Weight,
                FitnessStatus = model.FitnessStatus
            });
        }
        else
        {
            fitnessRecord.Weight = model.Weight;
            fitnessRecord.FitnessStatus = model.FitnessStatus;
        }
        unitOfWork.SaveChanges();
        return new APIResponse() { IsSuccess = true, Message = FitnessTrackingRes.Message_UserWeightRecordUpdated };
    }

    /// <summary>
    /// Gets the user fitness data by date asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns user fitness details for the date.</returns>
    public async Task<APIResponse> GetUserFitnessDataByDateAsync(FitnessDetailsFilterModel model)
    {
        var fitnessRecord = await unitOfWork.GetGenericRepository<UserFitnessDetail>().GetOneAsync(x =>
            x.RecordDate.Year == model.RecordDate.Year &&
            x.RecordDate.Month == model.RecordDate.Month &&
            x.RecordDate.Date == model.RecordDate.Date);
        if (fitnessRecord == null)
            return new APIResponse() { IsSuccess = false };
        return new APIResponse() { IsSuccess = true, Data = mapper.Map<ViewFitnessModel>(fitnessRecord) };
    }

    /// <summary>
    /// Gets the fitness details for period asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns list of records.</returns>
    public APIResponse GetFitnessDetailsForPeriodAsync(FitnessDetailsFilterModel model)
    {
        var startDate = model.RecordDate.AddMonths(-1);
        var endDate = model.RecordDate.AddMonths(1);
        var fitnessRecords = unitOfWork.GetGenericRepository<UserFitnessDetail>().GetQueryable(x =>
            x.RecordDate >= startDate && x.RecordDate <= endDate, null);
        var fitnessDetails = mapper.Map<List<ViewFitnessModel>>(fitnessRecords);
        GetBurnCalories(startDate, endDate, ref fitnessDetails);
        GetGainCalories(startDate, endDate, ref fitnessDetails);
        return new APIResponse() { IsSuccess = true, Data = fitnessDetails };
    }

    private void GetBurnCalories(DateTime startDate, DateTime endDate, ref List<ViewFitnessModel> fitnessDetails)
    {
        var workoutPlans = unitOfWork.GetGenericRepository<WorkoutPlan>()
            .GetQueryable(x => x.StartDate >= startDate || x.EndDate <= endDate, null)
            .Include(x => x.WorkoutPlanRoutines)
            .ToList();
        foreach (var plan in workoutPlans)
        {
            if (plan.OccurrenceType == OccurrenceTypes.OneTime)
            {
                var fitnessDetail = fitnessDetails.FirstOrDefault(x =>
                x.RecordDate.Year == plan.StartDate.Year &&
                x.RecordDate.Month == plan.StartDate.Month &&
                x.RecordDate.Date == plan.StartDate.Date);
                if (fitnessDetail == null)
                {
                    fitnessDetail = new ViewFitnessModel() { RecordDate = plan.StartDate };
                    fitnessDetails.Add(fitnessDetail);
                }
                fitnessDetail.CaloriesBurn += plan.WorkoutPlanRoutines.Sum(x => x.BurnCalories);
            }
            else
            {
                var planStartDate = plan.StartDate;
                do
                {
                    var fitnessDetail = fitnessDetails.FirstOrDefault(x =>
                    x.RecordDate.Year == planStartDate.Year &&
                    x.RecordDate.Month == planStartDate.Month &&
                    x.RecordDate.Date == planStartDate.Date);
                    if (fitnessDetail == null)
                    {
                        fitnessDetail = new ViewFitnessModel() { RecordDate = planStartDate };
                        fitnessDetails.Add(fitnessDetail);
                    }
                    fitnessDetail.CaloriesBurn += plan.WorkoutPlanRoutines.Sum(x => x.BurnCalories);
                    planStartDate = planStartDate.AddDays(1);

                } while (planStartDate.ToShortDateString() != plan.EndDate.AddDays(1).ToShortDateString());
            }

        }
    }

    private void GetGainCalories(DateTime startDate, DateTime endDate, ref List<ViewFitnessModel> fitnessDetails)
    {
        var cheatMealPlans = unitOfWork.GetGenericRepository<CheatMealPlan>()
            .GetQueryable(x => x.StartDate >= startDate || x.EndDate <= endDate, null)
            .Include(x => x.CheatMealPlanFoods)
            .ToList();
        foreach (var plan in cheatMealPlans)
        {
            if (plan.OccurrenceType == OccurrenceTypes.OneTime)
            {
                var fitnessDetail = fitnessDetails.FirstOrDefault(x =>
                x.RecordDate.Year == plan.StartDate.Year &&
                x.RecordDate.Month == plan.StartDate.Month &&
                x.RecordDate.Date == plan.StartDate.Date);
                if (fitnessDetail == null)
                {
                    fitnessDetail = new ViewFitnessModel() { RecordDate = plan.StartDate };
                    fitnessDetails.Add(fitnessDetail);
                }
                fitnessDetail.CaloriesGain += plan.CheatMealPlanFoods.Sum(x => x.CaloriesGain);
            }
            else
            {
                var planStartDate = plan.StartDate;
                do
                {
                    var fitnessDetail = fitnessDetails.FirstOrDefault(x =>
                    x.RecordDate.Year == planStartDate.Year &&
                    x.RecordDate.Month == planStartDate.Month &&
                    x.RecordDate.Date == planStartDate.Date);
                    if (fitnessDetail == null)
                    {
                        fitnessDetail = new ViewFitnessModel() { RecordDate = planStartDate };
                        fitnessDetails.Add(fitnessDetail);
                    }
                    fitnessDetail.CaloriesGain += plan.CheatMealPlanFoods.Sum(x => x.CaloriesGain);
                    planStartDate = planStartDate.AddDays(1);
                } while (planStartDate.ToShortDateString() != plan.EndDate.AddDays(1).ToShortDateString());
            }

        }
    }
}
