using AutoMapper;
using W1986411.EAD.Data;
using W1986411.EAD.Model;
using W1986411.EAD.Core;

namespace W1986411.EAD.Service;

/// <summary>
/// Workout profile.
/// </summary>
/// <seealso cref="Profile" />
public class WorkoutProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkoutProfile"/> class.
    /// </summary>
    public WorkoutProfile() {
        CreateMap<InsertUpdateWorkoutModel, WorkoutPlan>().ReverseMap();
        CreateMap<InsertUpdateWorkoutRoutineModel, WorkoutPlanRoutine>().ReverseMap();
        CreateMap<WorkoutPlan, ViewWorkoutPlanModel>()
            .ForMember(dist => dist.PlanId, s => s.MapFrom(o => o.Id))
            .ForMember(dist => dist.PlanName, s => s.MapFrom(o => o.WorkoutName))
            .ForMember(dist => dist.occurrenceType, s => s.MapFrom(o => o.OccurrenceType.GetEnumDisplayName()))
            .ForMember(dist => dist.CaloriesBurn, s => s.MapFrom(o => GetCaloriesBurn(o.WorkoutPlanRoutines.ToList())));
        CreateMap<WorkoutPlan, WorkoutPlanModel>();
        CreateMap<WorkoutPlanRoutine, WorkoutPlanRoutineModel>();
    }

    /// <summary>
    /// Gets the calories burn.
    /// </summary>
    /// <param name="workoutPlanRoutines">The workout plan routines.</param>
    /// <returns>Returns calories burn.</returns>
    private double GetCaloriesBurn(List<WorkoutPlanRoutine> workoutPlanRoutines)
    {
        var caloriesBurn = (double)default;
        foreach (var plan in workoutPlanRoutines)
            caloriesBurn += plan.BurnCalories;
        return caloriesBurn;
    }
}
