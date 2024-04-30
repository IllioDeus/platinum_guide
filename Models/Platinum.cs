namespace Plats.Models;
public record Platinum
{
    public Guid Id { get; init; }
    public string GameName { get; init; }
    public string TrophyName { get; init; }
    public int Difficulty { get; init; }
    public float AverageTime { get; init; }

    public Platinum(Guid id, string gameName, string trophyName, int difficulty, float averageTime)
    {
        Id = id;
        GameName = gameName;
        TrophyName = trophyName;
        Difficulty = difficulty;
        AverageTime = averageTime;
    }
}
