using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Principal;
using System.Text;
using W1986411.EAD.Data;
using W1986411.EAD.Service;
using W1986411.EAD.UI;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var Configuration = builder.Configuration;
// Add services to the container.

//Service classes dependency inject
var businessProjectClasses = typeof(IUserService).Assembly.GetTypes()
             .Where(service => service.Name.EndsWith("Service")).ToList();
var interfaces = businessProjectClasses.Where(x => x.IsInterface);
var serviceClasses = businessProjectClasses.Where(x => !x.IsInterface);
foreach (var serviceClass in serviceClasses)
{
    var interfaceClass = interfaces.Where(x => x.Name.Equals($"I{serviceClass.Name}")).FirstOrDefault();
    if (interfaceClass != null)
        services.AddTransient(interfaceClass, serviceClass);
}

services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);
services.AddTransient<IUnitOfWork, UnitOfWork>();

//Auto mapper
var mapper = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new UserProfile());
    cfg.AddProfile(new WorkoutProfile());
    cfg.AddProfile(new CheatMealProfile());
    cfg.AddProfile(new FitnessProfile());
});
services.AddSingleton(mapper.CreateMapper());

//Swagger
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fitness Tracking", Version = "v1" });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Bearer token using the Bearer scheme. Example: \"Bearer {token}\"",
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
    };
    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

services.AddControllersWithViews();

//DB Context
services.AddDbContext<APIDBContext>(options =>
{
    options.UseInMemoryDatabase("fitnesstrackingdb");
});
services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<APIDBContext>()
    .AddDefaultTokenProviders();

//Authentication
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = Configuration["JWT:ValidAudience"],
        ValidIssuer = Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
    };
});
services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
});

//Logs
Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(
                    "logs/fitness_tracking_.txt",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day
                )
                .CreateLogger();
Log.Information("started");

//Seed Data
DBConfiguration.SeedData(services.BuildServiceProvider());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fitness Tracking API V1");
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
