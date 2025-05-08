using System.ComponentModel.DataAnnotations.Schema;
using HT.Domain.Entities.Base;

namespace HT.Domain.Entities;

public class JournalLog : Entity
{
    public JournalLog() { }

    public JournalLog(Guid userId, DateTime date, int healthScore, int energyScore, int moodScore,
        ICollection<HabitLog> habitLogs)
    {
        UserId = userId;
        Date = date;
        HealthScore = healthScore;
        EnergyScore = energyScore;
        MoodScore = moodScore;
        HabitLogs = habitLogs;
    }

    public Guid UserId { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public ICollection<HabitLog> HabitLogs { get; set; }
    public int HealthScore { get; set; }
    public int EnergyScore { get; set; }
    public int MoodScore { get; set; }

    [NotMapped] public int Score => HealthScore + EnergyScore + MoodScore;

    public void UpdateScores(int health, int energy, int mood)
    {
        HealthScore = health;
        EnergyScore = energy;
        MoodScore = mood;
    }
}