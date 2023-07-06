﻿using System.Security.Claims;
using W1986411.EAD.Core;
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
        var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();

        //Register User
        userService.RegisterUserAsync(new InsertUpdateUserModel()
        {
            Email = "testuser@gmail.com",
            FirstName = "test",
            LastName = "user",
            Height = 160,
            Password = "abcABC@123"
        });
        var loginUser = userService.GetUserByEmailAsync("testuser@gmail.com").Result;

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, $"{loginUser.FirstName} {loginUser.LastName}"),
            new Claim(ClaimTypes.Name, loginUser.Id),
            new Claim(ClaimTypes.Email, loginUser.Email)
        };
        httpContextAccessor.HttpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(authClaims, "CustomAuth"))
        };

        var workoutService = provider.GetRequiredService<IWorkoutService>();

        //Create Workout Plans
        workoutService.InsertWorkoutPlanAsync(new InsertUpdateWorkoutModel()
        {
            WorkoutName = "Test Plan 1",
            IsActive = true,
            StartDate = new DateTime(2023, 4, 01),
            EndDate = new DateTime(2023, 4, 30),
            OccurrenceType = OccurrenceTypes.Recurring,
            Routines = new List<InsertUpdateWorkoutRoutineModel>()
            {
                new InsertUpdateWorkoutRoutineModel()
                {
                    Name = "Test Plan 1 - Routine 1",
                    IsActive = true,
                    BurnCalories = 500,
                    Sets = 30,
                    Reps = 10
                },
                new InsertUpdateWorkoutRoutineModel()
                {
                    Name = "Test Plan 1 - Routine 2",
                    IsActive = true,
                    BurnCalories = 600,
                    Sets = 20,
                    Reps = 20
                }
            }
        });
        workoutService.InsertWorkoutPlanAsync(new InsertUpdateWorkoutModel()
        {
            WorkoutName = "Test Plan 2",
            IsActive = true,
            StartDate = new DateTime(2023, 4, 15),
            EndDate = new DateTime(2023, 4, 15),
            OccurrenceType = OccurrenceTypes.OneTime,
            Routines = new List<InsertUpdateWorkoutRoutineModel>()
            {
                new InsertUpdateWorkoutRoutineModel()
                {
                    Name = "Test Plan 2 - Routine 1",
                    IsActive = true,
                    BurnCalories = 800,
                    Sets = 50,
                    Reps = 10
                },
                new InsertUpdateWorkoutRoutineModel()
                {
                    Name = "Test Plan 2 - Routine 2",
                    IsActive = true,
                    BurnCalories = 800,
                    Sets = 30,
                    Reps = 10
                }
            }
        });

        //Create Cheat Meal Plans
    }
}
