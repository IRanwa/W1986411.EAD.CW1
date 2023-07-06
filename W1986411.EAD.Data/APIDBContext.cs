using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace W1986411.EAD.Data;

/// <summary>
/// API DB Context.
/// </summary>
/// <seealso cref="IdentityDbContext&lt;ApplicationUser&gt;" />
public class APIDBContext : IdentityDbContext<ApplicationUser>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="APIDBContext"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public APIDBContext(DbContextOptions<APIDBContext> options) : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUser { get; set; }
    public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
    public DbSet<WorkoutPlanRoutine> WorkoutPlanRoutines { get; set; }
    public DbSet<CheatMealPlan> CheatMealPlans { get; set; }
    public DbSet<CheatMealPlanFood> CheatMealPlanFoods { get; set; }
}
