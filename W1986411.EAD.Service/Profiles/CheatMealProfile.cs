using AutoMapper;
using W1986411.EAD.Core;
using W1986411.EAD.Data;
using W1986411.EAD.Model;

namespace W1986411.EAD.Service;

/// <summary>
/// Cheat meal profile.
/// </summary>
/// <seealso cref="Profile" />
public class CheatMealProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CheatMealProfile"/> class.
    /// </summary>
    public CheatMealProfile()
    {
        CreateMap<CheatMealPlan, InsertUpdateCheatMealModel>().ReverseMap();
        CreateMap<CheatMealPlanFood, InsertUpdateCheatMealFoodModel>().ReverseMap();
        CreateMap<CheatMealPlan, CheatMealPlanModel>();
        CreateMap<CheatMealPlanFood, CheatMealPlanFoodModel>();
        CreateMap<CheatMealPlan, ViewCheatMealPlanModel>()
            .ForMember(dist => dist.Id, s => s.MapFrom(o => o.Id))
            .ForMember(dist => dist.Name, s => s.MapFrom(o => o.CheatMealName))
            .ForMember(dist => dist.OccurrenceType, s => s.MapFrom(o => o.OccurrenceType.GetEnumDisplayName()))
            .ForMember(dist => dist.CaloriesGain, s => s.MapFrom(o => GetCaloriesGain(o.CheatMealPlanFoods)));
    }

    /// <summary>
    /// Gets the calories gain.
    /// </summary>
    /// <param name="cheatMealPlanFoods">The cheat meal plan foods.</param>
    /// <returns>Returns total calories gain.</returns>
    private double GetCaloriesGain(ICollection<CheatMealPlanFood> cheatMealPlanFoods)
    {
        var caloriesGain = (double)default;
        foreach (var food in cheatMealPlanFoods)
            caloriesGain += food.CaloriesGain;
        return caloriesGain;
    }
}