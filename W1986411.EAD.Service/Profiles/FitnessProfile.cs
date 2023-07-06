using AutoMapper;
using W1986411.EAD.Core;
using W1986411.EAD.Data;
using W1986411.EAD.Model;

namespace W1986411.EAD.Service;

/// <summary>
/// Fitness profile.
/// </summary>
/// <seealso cref="Profile" />
public class FitnessProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FitnessProfile"/> class.
    /// </summary>
    public FitnessProfile()
    {
        CreateMap<InsertUpdateFitnessModel, UserFitnessDetail>();
        CreateMap<UserFitnessDetail, ViewFitnessModel>()
            .ForMember(dest => dest.FitnessStatusStr, o => o.MapFrom(src => src.FitnessStatus.GetEnumDisplayName()));
    }
}
