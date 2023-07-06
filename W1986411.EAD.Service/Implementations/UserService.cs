using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using W1986411.EAD.Core;
using W1986411.EAD.Data;
using W1986411.EAD.Model;

namespace W1986411.EAD.Service;

/// <summary>
/// User service.
/// </summary>
/// <seealso cref="IUserService" />
public class UserService : IUserService
{
    /// <summary>
    /// The mapper
    /// </summary>
    private readonly IMapper mapper;

    /// <summary>
    /// The user manager
    /// </summary>
    private readonly UserManager<ApplicationUser> userManager;

    /// <summary>
    /// The unit of work
    /// </summary>
    private readonly IUnitOfWork unitOfWork;

    /// <summary>
    /// The configuration
    /// </summary>
    private readonly IConfiguration configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="mapper">The mapper.</param>
    /// <param name="userManager">The user manager.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="configuration">The configuration.</param>
    public UserService(
        IMapper mapper,
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork,
        IConfiguration configuration
        ) { 
        this.mapper = mapper;
        this.userManager = userManager;
        this.unitOfWork = unitOfWork;
        this.configuration = configuration;
    }

    /// <summary>
    /// Registers the user.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns user register response.</returns>
    public async Task<APIResponse> RegisterUserAsync(InsertUpdateUserModel model)
    {
        var user = mapper.Map<ApplicationUser>(model);
        user.Id = Guid.NewGuid().ToString();
        user.UserName = model.Email;
        var identityResult = await userManager.CreateAsync(user, model.Password);
        if (identityResult.Succeeded)
            return new APIResponse() { IsSuccess = true, Message =  FitnessTrackingRes.Message_UserRegistrationSuccess };
        return new APIResponse() { IsSuccess = false, Message = FitnessTrackingRes.Message_UserRegistrationFailed };
    }

    /// <summary>
    /// Logins the user asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns login response.</returns>
    public async Task<APIResponse> LoginUserAsync(LoginUserModel model)
    {
        var loginUser = await unitOfWork.GetGenericRepository<ApplicationUser>()
            .GetOneAsync(userInfo => userInfo.Email == model.Email);
        if(loginUser == null)
            return new APIResponse() { IsSuccess = false, Message = FitnessTrackingRes.Message_UserLoginCredentialsInvalid };
        var loginStatus = await userManager.CheckPasswordAsync(loginUser, model.Password);
        if(!loginStatus)
            return new APIResponse() { IsSuccess = false, Message = FitnessTrackingRes.Message_UserLoginCredentialsInvalid };
        var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, $"{loginUser.FirstName} {loginUser.LastName}"),
                    new Claim(ClaimTypes.Name, loginUser.Id),
                    new Claim(ClaimTypes.Email, loginUser.Email)
                };
        var token = GetToken(authClaims);
        return new APIResponse() { IsSuccess = true, Data = new JwtSecurityTokenHandler().WriteToken(token) };
    }

    /// <summary>
    /// Gets the token.
    /// </summary>
    /// <param name="authClaims">The authentication claims.</param>
    /// <returns>Returns JWT token.</returns>
    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: configuration["JWT:ValidIssuer"],
            audience: configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddMinutes(int.Parse(configuration["JWT:ExpiresInMinutes"])),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
        return token;
    }
}
