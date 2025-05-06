using HT.Application.Interfaces;
using HT.Infrastructure.Auth;
using HT.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();

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
builder.Services.AddScoped<IHabitService, HabitService>();
builder.Services.AddScoped<IUserHabitService, UserHabitService>();
builder.Services.AddScoped<IInsightService, InsightService>();
builder.Services.AddScoped<IUserJournalService, UserJournalService>();

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