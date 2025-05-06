using System.Text.Json;
using HT.Application.Interfaces;
using HT.Domain.Repositories;
using HT.Infrastructure.Auth;
using HT.Infrastructure.Persistence;
using HT.Infrastructure.Persistence.Habits;
using HT.Infrastructure.Persistence.Insights;
using HT.Infrastructure.Persistence.Journals;
using HT.Infrastructure.Persistence.ML;
using HT.Infrastructure.Persistence.UserHabits;
using HT.Infrastructure.Persistence.Users;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("VueFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<HtContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<DatabaseInitializer>();
builder.Services.AddScoped<NeuralEngine>();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IHabitService, HabitService>();
builder.Services.AddScoped<IUserHabitService, UserHabitService>();
builder.Services.AddScoped<IUserHabitRepository, UserHabitRepository>();
builder.Services.AddScoped<IInsightService, InsightService>();
builder.Services.AddScoped<IUserJournalService, UserJournalService>();
builder.Services.AddScoped<IUserJournalRepository, UserJournalRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<HtContext>();
await context.Database.MigrateAsync();

var databaseInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.SeedAsync();

app.UseCors("VueFrontend");

app.MapControllers();
app.Run();