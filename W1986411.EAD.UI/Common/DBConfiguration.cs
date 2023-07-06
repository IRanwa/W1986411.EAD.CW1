using W1986411.EAD.Model;
using W1986411.EAD.Service;

namespace W1986411.EAD.UI;

/// <summary>
/// DB Configuration.
/// </summary>
public static class DBConfiguration
{
    /// <summary>
    /// Seeds the data.
    /// </summary>
    /// <param name="provider">The provider.</param>
    public static void SeedData(ServiceProvider provider)
    {
        var userService = provider.GetRequiredService<IUserService>();

        //Register User
        userService.RegisterUserAsync(new InsertUpdateUserModel()
        {
            Email = "testuser@gmail.com",
            FirstName = "test",
            LastName = "user",
            Height = 160,
            Password = "abcABC@123"
        });

        //Create Workout Plans

        //Create Cheat Meal Plans
    }
}
