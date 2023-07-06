using AutoMapper;
using W1986411.EAD.Data;
using W1986411.EAD.Model;

namespace W1986411.EAD.Service;

/// <summary>
/// User profile.
/// </summary>
/// <seealso cref="Profile" />
public class UserProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserProfile"/> class.
    /// </summary>
    public UserProfile() {
        CreateMap<InsertUpdateUserModel, ApplicationUser>();
    }
}
