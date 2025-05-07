using System.Text;
using HT.Application.Interfaces;
using HT.Infrastructure.Auth;
using HT.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("VueFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:8081")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var jwtSecretKey = builder.Configuration["Auth:Jwt:SecretKey"];
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey!)),
            ValidIssuer = "https://localhost:5001",
            ValidAudience = "https://localhost:8081"
        };
        
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine("JWT failed: " + ctx.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = ctx =>
            {
                Console.WriteLine("JWT succeeded for: " + ctx.Principal.Identity?.Name);
                return Task.CompletedTask;
            }
        };

    })
    .AddCookie()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Auth:Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Auth:Google:ClientSecret"]!;
    });
builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<HtContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<DatabaseInitializer>();
builder.Services.AddScoped<NeuralEngine>();
builder.Services.AddScoped<JwtService>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IHabitService, HabitService>();
builder.Services.AddScoped<IUserHabitService, UserHabitService>();
builder.Services.AddScoped<IInsightService, InsightService>();
builder.Services.AddScoped<IUserJournalService, UserJournalService>();

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