using AutoMapper;
using W1986411.EAD.Data;
using W1986411.EAD.Model;

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
    }
}
