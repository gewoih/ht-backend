using System.Text;
using HT.Application.Interfaces;
using HT.Domain.Entities.Identity;
using HT.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddProblemDetails();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddSerilog();

builder.Services.AddCors(options =>
{
    options.AddPolicy("VueFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:8080")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddIdentity<User, Role>(o =>
    {
        o.User.RequireUniqueEmail = true;
        o.SignIn.RequireConfirmedEmail = true;
        o.Password.RequireNonAlphanumeric = false;
        o.Password.RequiredUniqueChars = 4;
        o.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<HtContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthService.JwtSecretKey)),
            ValidIssuer = AuthService.ValidIssuer,
            ValidAudience = AuthService.ValidAudience,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<HtContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<DatabaseInitializer>();
builder.Services.AddScoped<NeuralEngine>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IHabitService, HabitService>();
builder.Services.AddScoped<IUserHabitService, UserHabitService>();
builder.Services.AddScoped<IInsightService, InsightService>();
builder.Services.AddScoped<IUserJournalService, UserJournalService>();
builder.Services.AddScoped<IUserAnalyticsService, UserAnalyticsService>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();

if (builder.Environment.IsDevelopment())
    builder.Services.AddScoped<IEmailService, FakeEmailService>();
else
    builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<HtContext>();
await context.Database.MigrateAsync();

var databaseInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.SeedAsync();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("VueFrontend");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();