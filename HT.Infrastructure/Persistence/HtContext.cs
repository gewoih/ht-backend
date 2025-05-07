using HT.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HT.Infrastructure.Persistence;

public class HtContext(DbContextOptions<HtContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Habit> Habits { get; set; }
    public DbSet<UserHabit> UserHabits { get; set; }
    public DbSet<HabitLog> HabitLogs { get; set; }
    public DbSet<JournalLog> JournalLogs { get; set; }
}