using System.ComponentModel.DataAnnotations.Schema;

namespace HT.Domain.ValueObjects;

[ComplexType]
public record DailyScore(int Health, int Energy, int Mood)
{
    public int Total => Health + Energy + Mood;
}