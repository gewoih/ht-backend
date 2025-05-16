using HT.Domain.Entities;
using HT.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HT.Infrastructure.Persistence;

public class HtContext(DbContextOptions<HtContext> options) : IdentityDbContext<User, Role, Guid>(options)
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<EmailConfirmationCode> EmailConfirmationCodes { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Habit> Habits { get; set; }
    public DbSet<UserHabit> UserHabits { get; set; }
    public DbSet<HabitLog> HabitLogs { get; set; }
    public DbSet<JournalLog> JournalLogs { get; set; }
}