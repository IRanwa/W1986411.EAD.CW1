using AutoMapper;
using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
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
            x.RecordDate.Date == model.RecordDate.Date &&
            x.UserId == user.Identity.Name);
        if (fitnessRecord == null)
        {
            await unitOfWork.GetGenericRepository<UserFitnessDetail>().Add(new UserFitnessDetail()
            {
                RecordDate = model.RecordDate,
                UserId = user.Identity.Name,
                Weight = model.Weight,
                FitnessStatus = model.FitnessStatus,
                IsActive = model.IsActive
            });
        }
        else
        {
            fitnessRecord.Weight = model.Weight;
            fitnessRecord.FitnessStatus = model.FitnessStatus;
            fitnessRecord.IsActive = model.IsActive;
        }
        unitOfWork.SaveChanges(user);
        return new APIResponse() { IsSuccess = true, Message = FitnessTrackingRes.Message_UserWeightRecordUpdated };
    }

    /// <summary>
    /// Removes the fitness details asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns response.</returns>
    public async Task<APIResponse> RemoveFitnessDetailsAsync(FitnessDetailsFilterModel model)
    {
        var fitnessRecord = await unitOfWork.GetGenericRepository<UserFitnessDetail>().GetOneAsync(x =>
           x.RecordDate.Year == model.RecordDate.Year &&
           x.RecordDate.Month == model.RecordDate.Month &&
           x.RecordDate.Date == model.RecordDate.Date &&
           x.IsActive &&
           x.UserId == user.Identity.Name);
        if (fitnessRecord == null)
            return new APIResponse() { IsSuccess = false, Message = FitnessTrackingRes.Message_FitnessInfoRemoveFailed};
        fitnessRecord.IsActive = false;
        unitOfWork.SaveChanges(user);
        return new APIResponse() { IsSuccess = true, Message = FitnessTrackingRes.Message_FitnessInfoRemoveSuccess };
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
            x.RecordDate.Date == model.RecordDate.Date &&
            x.IsActive &&
            x.UserId == user.Identity.Name);
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
            (x.RecordDate >= startDate || x.RecordDate <= endDate) &&
            x.IsActive &&
            x.UserId == user.Identity.Name, null);
        var fitnessDetails = mapper.Map<List<ViewFitnessModel>>(fitnessRecords);
        GetBurnCalories(startDate, endDate, ref fitnessDetails);
        GetGainCalories(startDate, endDate, ref fitnessDetails);
        PredictFitnessStausAndWeight(ref fitnessDetails);
        return new APIResponse() { IsSuccess = true, Data = fitnessDetails };
    }

    /// <summary>
    /// Gets the burn calories.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <param name="fitnessDetails">The fitness details.</param>
    private void GetBurnCalories(DateTime startDate, DateTime endDate, ref List<ViewFitnessModel> fitnessDetails)
    {
        var workoutPlans = unitOfWork.GetGenericRepository<WorkoutPlan>()
            .GetQueryable(x => (x.StartDate >= startDate || x.EndDate <= endDate)  &&
            x.UserId == user.Identity.Name, null)
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
        PredictCaloriesBurn(workoutPlans, ref fitnessDetails);
    }

    /// <summary>
    /// Gets the gain calories.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <param name="fitnessDetails">The fitness details.</param>
    private void GetGainCalories(DateTime startDate, DateTime endDate, ref List<ViewFitnessModel> fitnessDetails)
    {
        var cheatMealPlans = unitOfWork.GetGenericRepository<CheatMealPlan>()
            .GetQueryable(x => (x.StartDate >= startDate || x.EndDate <= endDate) &&
                x.UserId == user.Identity.Name, null)
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
        PredictCaloriesGain(cheatMealPlans, ref fitnessDetails);
    }

    /// <summary>
    /// Predicts the calories burn.
    /// </summary>
    /// <param name="workoutPlans">The workout plans.</param>
    /// <param name="fitnessDetails">The fitness details.</param>
    private void PredictCaloriesBurn(List<WorkoutPlan> workoutPlans, ref List<ViewFitnessModel> fitnessDetails)
    {
        var startDate = DateTime.Now.AddDays(1);
        var endDate = startDate.AddMonths(1);
        do
        {
            var beginDate = startDate.AddDays(-7);
            var workoutPlansForPeriod = workoutPlans.Where(x => ((x.StartDate <= beginDate && beginDate <= x.EndDate) || 
                (x.StartDate >= beginDate && startDate >= x.EndDate))
                && x.IsActive && x.UserId == user.Identity.Name).ToList();
            var totalCaloriesBurnPreviousWeek = 0.0;
            var days = new Dictionary<string, string>();
            foreach(var plan in workoutPlansForPeriod)
            {
                if (plan.OccurrenceType == OccurrenceTypes.OneTime) {
                    totalCaloriesBurnPreviousWeek += plan.WorkoutPlanRoutines.Sum(x => x.BurnCalories);
                    days.TryAdd(plan.StartDate.ToShortDateString(), plan.StartDate.ToShortDateString());
                }
                else
                {
                    var planStartDate = beginDate;
                    var planEndDate = plan.EndDate < DateTime.Now ? plan.EndDate.AddDays(1) : DateTime.Now;
                    do
                    {
                        totalCaloriesBurnPreviousWeek += plan.WorkoutPlanRoutines.Sum(x => x.BurnCalories);
                        days.TryAdd(planStartDate.ToShortDateString(), planStartDate.ToShortDateString());
                        planStartDate = planStartDate.AddDays(1);
                    } while (planStartDate.ToShortDateString() != planEndDate.ToShortDateString());
                }
            }
            if (totalCaloriesBurnPreviousWeek == 0)
            {
                startDate = startDate.AddDays(1);
                continue;
            }
            var avgCaloriesBurn = Math.Round(totalCaloriesBurnPreviousWeek / days.Count, 2);
            workoutPlans.Add(new WorkoutPlan()
            {
                EndDate = startDate,
                StartDate = startDate,
                IsActive = true,
                OccurrenceType = OccurrenceTypes.OneTime,
                UserId = user.Identity.Name,
                WorkoutPlanRoutines = new List<WorkoutPlanRoutine>()
                {
                    new WorkoutPlanRoutine()
                    {
                        BurnCalories = avgCaloriesBurn
                    }
                }
            });

            var fitnessDetail = fitnessDetails.FirstOrDefault(x =>
                    x.RecordDate.Year == startDate.Year &&
                    x.RecordDate.Month == startDate.Month &&
                    x.RecordDate.Date == startDate.Date);
            if (fitnessDetail == null)
            {
                fitnessDetail = new ViewFitnessModel() { RecordDate = startDate };
                fitnessDetails.Add(fitnessDetail);
            }
            fitnessDetail.PredCaloriesBurn = avgCaloriesBurn;
            startDate = startDate.AddDays(1);
        } while (startDate.ToShortDateString() != endDate.ToShortDateString());
    }

    /// <summary>
    /// Predicts the calories gain.
    /// </summary>
    /// <param name="cheatMealPlans">The cheat meal plans.</param>
    /// <param name="fitnessDetails">The fitness details.</param>
    private void PredictCaloriesGain(List<CheatMealPlan> cheatMealPlans, ref List<ViewFitnessModel> fitnessDetails)
    {
        var startDate = DateTime.Now.AddDays(1);
        var endDate = startDate.AddMonths(1);
        do
        {
            var beginDate = startDate.AddDays(-7);
            var cheatMealsPlansForPeriod = cheatMealPlans.Where(x => ((x.StartDate <= beginDate && beginDate <= x.EndDate) ||
                (x.StartDate >= beginDate && startDate >= x.EndDate))
                && x.IsActive && x.UserId == user.Identity.Name).ToList();
            var totalCaloriesGainPreviousWeek = 0.0;
            var days = new Dictionary<string, string>();
            foreach (var plan in cheatMealsPlansForPeriod)
            {
                if (plan.OccurrenceType == OccurrenceTypes.OneTime)
                {
                    totalCaloriesGainPreviousWeek += plan.CheatMealPlanFoods.Sum(x => x.CaloriesGain);
                    days.TryAdd(plan.StartDate.ToShortDateString(), plan.StartDate.ToShortDateString());
                }
                else
                {
                    var planStartDate = beginDate;
                    var planEndDate = plan.EndDate < DateTime.Now ? plan.EndDate.AddDays(1) : DateTime.Now;
                    do
                    {
                        totalCaloriesGainPreviousWeek += plan.CheatMealPlanFoods.Sum(x => x.CaloriesGain);
                        days.TryAdd(planStartDate.ToShortDateString(), planStartDate.ToShortDateString());
                        planStartDate = planStartDate.AddDays(1);
                    } while (planStartDate.ToShortDateString() != planEndDate.ToShortDateString());
                }
            }
            if (totalCaloriesGainPreviousWeek == 0)
            {
                startDate = startDate.AddDays(1);
                continue;
            }
            var avgCaloriesGain = Math.Round(totalCaloriesGainPreviousWeek / days.Count, 2);
            cheatMealPlans.Add(new CheatMealPlan()
            {
                EndDate = startDate,
                StartDate = startDate,
                IsActive = true,
                OccurrenceType = OccurrenceTypes.OneTime,
                UserId = user.Identity.Name,
                CheatMealPlanFoods = new List<CheatMealPlanFood>()
                {
                    new CheatMealPlanFood()
                    {
                        CaloriesGain = avgCaloriesGain
                    }
                }
            });

            var fitnessDetail = fitnessDetails.FirstOrDefault(x =>
                    x.RecordDate.Year == startDate.Year &&
                    x.RecordDate.Month == startDate.Month &&
                    x.RecordDate.Date == startDate.Date);
            if (fitnessDetail == null)
            {
                fitnessDetail = new ViewFitnessModel() { RecordDate = startDate };
                fitnessDetails.Add(fitnessDetail);
            }
            fitnessDetail.PredCaloriesGain = avgCaloriesGain;
            startDate = startDate.AddDays(1);
        } while (startDate.ToShortDateString() != endDate.ToShortDateString());
    }

    /// <summary>
    /// Predicts the fitness staus and weight.
    /// </summary>
    /// <param name="fitnessDetails">The fitness details.</param>
    private void PredictFitnessStausAndWeight(ref List<ViewFitnessModel> fitnessDetails)
    {
        var startDate = DateTime.Now.AddDays(1);
        var endDate = startDate.AddMonths(1);

        var userFitnessDetails = unitOfWork.GetGenericRepository<UserFitnessDetail>()
            .GetQueryable(x => 
                x.RecordDate >= startDate.AddDays(-7) && x.RecordDate <= startDate &&
                x.UserId == user.Identity.Name && x.IsActive, null)
            .ToList();

        do
        {
            var beginDate = startDate.AddDays(-7);
            var fitnessWeekDataList = userFitnessDetails
                .Where(x => x.RecordDate >= beginDate && x.RecordDate <= startDate).ToList();
            if (!fitnessWeekDataList.Any())
            {
                startDate = startDate.AddDays(1);
                continue;
            }
            var predUserFitness = GetFitnessPrediction(fitnessWeekDataList);
            var predWeight = GetPredictedWeight(fitnessWeekDataList);
            userFitnessDetails.Add(new UserFitnessDetail()
            {
                RecordDate = startDate,
                IsActive = true,
                UserId = user.Identity.Name,
                FitnessStatus = predUserFitness,
                Weight = predWeight
            });
            var fitnessDetail = fitnessDetails.FirstOrDefault(x =>
                    x.RecordDate.Year == startDate.Year &&
                    x.RecordDate.Month == startDate.Month &&
                    x.RecordDate.Date == startDate.Date);
            if (fitnessDetail == null)
            {
                fitnessDetail = new ViewFitnessModel() { RecordDate = startDate };
                fitnessDetails.Add(fitnessDetail);
            }
            fitnessDetail.PredFitnessStatusStr = predUserFitness.GetEnumDisplayName();
            fitnessDetail.PredWeight = predWeight;
            startDate = startDate.AddDays(1);
        } while (startDate.ToShortDateString() != endDate.ToShortDateString());
    }

    /// <summary>
    /// Gets the fitness prediction.
    /// </summary>
    /// <param name="fitnessWeekDataList">The fitness week data list.</param>
    /// <returns>Returns fitness sattus prediction.</returns>
    private FitnessStatus GetFitnessPrediction(IEnumerable<UserFitnessDetail> fitnessWeekDataList)
    {
        var fitnessPoints = 0;
        foreach (var fitnessWeekData in fitnessWeekDataList)
        {
            switch (fitnessWeekData.FitnessStatus)
            {
                case FitnessStatus.VeryPoor:
                    fitnessPoints -= 3;
                    break;
                case FitnessStatus.Poor:
                    fitnessPoints -= 2;
                    break;
                case FitnessStatus.Fair:
                    fitnessPoints -= 1;
                    break;
                case FitnessStatus.Good:
                    fitnessPoints += 1;
                    break;
                case FitnessStatus.Excellent:
                    fitnessPoints += 2;
                    break;
                case FitnessStatus.Superior:
                    fitnessPoints += 3;
                    break;
            }
        }
        fitnessPoints = fitnessPoints / fitnessWeekDataList.Count();
        if (fitnessPoints <= -3)
            return FitnessStatus.VeryPoor;
        else if (fitnessPoints > -3 && fitnessPoints <= -2)
            return FitnessStatus.Poor;
        else if (fitnessPoints > -2 && fitnessPoints <= -1)
            return FitnessStatus.Fair;
        else if (fitnessPoints > -1 && fitnessPoints <= 1)
            return FitnessStatus.Good;
        else if (fitnessPoints > 1 && fitnessPoints <= 2)
            return FitnessStatus.Excellent;
        else
            return FitnessStatus.Superior;
    }

    /// <summary>
    /// Gets the predicted weight.
    /// </summary>
    /// <param name="fitnessWeekDataList">The fitness week data list.</param>
    /// <returns>Returns predicted weight.</returns>
    private double GetPredictedWeight(IEnumerable<UserFitnessDetail> fitnessWeekDataList)
    {
        var weight = 0.0;
        foreach (var fitnessWeekData in fitnessWeekDataList)
            weight += fitnessWeekData.Weight;
        return Math.Round(weight / fitnessWeekDataList.Count(),2);
    }
}
