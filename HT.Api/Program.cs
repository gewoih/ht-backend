using System.Text.Json;
using HT.Common.Database;
using HT.Common.Entities;
using HT.Common.Services;
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

builder.Services.AddScoped<HabitService>();
builder.Services.AddScoped<JournalService>();
builder.Services.AddScoped<NeuralEngine>();
builder.Services.AddScoped<InsightService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<HtContext>();
await context.Database.MigrateAsync();

if (!await context.Habits.AnyAsync())
{
    await context.Habits.AddRangeAsync(
        new Habit { Name = "Пил кофе?" },
        new Habit { Name = "Пил алкоголь?" },
        new Habit { Name = "Мастурбировал?" },
        new Habit { Name = "Делал растяжку?" },
        new Habit { Name = "Пил магний?" },
        new Habit { Name = "Пил пробиотик?" },
        new Habit { Name = "Работал допоздна?" },
        new Habit { Name = "Ел перед сном?" },
        new Habit { Name = "Принимал горячий душ перед сном?" },
        new Habit { Name = "Делил кровать с кем-то?" },
        new Habit { Name = "Спал в темной комнате?" },
        new Habit { Name = "Использовал телефон перед сном?" },
        new Habit { Name = "Был простужен?" },
        new Habit { Name = "Испытывал боль в суставах или скованность?" },
        new Habit { Name = "Испытывал боль в спине?" },
        new Habit { Name = "Испытывал головную боль?" },
        new Habit { Name = "Испытывал вздутие?" },
        new Habit { Name = "Испытывал стресс?" },
        new Habit { Name = "Вступал в сексуальную близость?" },
        new Habit { Name = "Употреблял мясо?" },
        new Habit { Name = "Употреблял фрукты и/или овощи?" },
        new Habit { Name = "Употреблял молочные продукты?" },
        new Habit { Name = "Употреблял добавленный сахар?" },
        new Habit { Name = "Контактировал с семьей или друзьями?" },
        new Habit { Name = "Избегал обработанной пищи?" },
        new Habit { Name = "Читал перед сном?" }
    );
}

if (!await context.Users.AnyAsync())
{
    await context.Users.AddAsync(new User
    {
        Name = "Никита"
    });
}

await context.SaveChangesAsync();

app.UseCors("VueFrontend");

app.MapControllers();
app.Run();