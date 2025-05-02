using HT.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace HT.Common.Database;

public class HtContext(DbContextOptions<HtContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Habit> Habits { get; set; }
    public DbSet<HabitLog> HabitLogs { get; set; }
    public DbSet<JournalLog> JournalLogs { get; set; }
}