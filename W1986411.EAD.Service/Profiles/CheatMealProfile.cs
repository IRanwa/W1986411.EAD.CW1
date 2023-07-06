using AutoMapper;
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
    }
}