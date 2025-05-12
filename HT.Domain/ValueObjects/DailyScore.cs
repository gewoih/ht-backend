using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HT.Domain.ValueObjects;

[ComplexType]
public record DailyScore(int Health, int Energy, int Mood, int Sleep, int Calmness, int Satisfaction)
{
    [JsonIgnore]
    public int Total => Health + Energy + Mood + Sleep + Calmness + Satisfaction;
}