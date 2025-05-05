using System.ComponentModel.DataAnnotations.Schema;
using HT.Domain.Entities.Base;

namespace HT.Domain.Entities;

public class JournalLog : Entity
{
    public Guid UserId { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public ICollection<HabitLog> HabitLogs { get; set; }
    public int HealthScore { get; set; }
    public int EnergyScore { get; set; }
    public int MoodScore { get; set; }
    
    [NotMapped]
    public int Score => HealthScore + EnergyScore + MoodScore;
    
    public void UpdateScores(int health, int energy, int mood)
    {
        HealthScore = health;
        EnergyScore = energy;
        MoodScore   = mood;
    }
}