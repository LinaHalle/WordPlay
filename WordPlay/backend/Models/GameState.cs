namespace Brainfart.Models;

public class GameState
{
    public Guid GameId { get; set; }
    public GameStatus Status { get; set; }
    public List<Player> Players { get; set; } = new();
    public List<string> Categories { get; set; } = new();
    public string CurrentLetter { get; set; } = "";
    public Dictionary<Guid, Dictionary<string, string>> Answers { get; set; } = new();
    public Dictionary<Guid, int> Scoreboard { get; set; } = new();
    public int Rounds { get; set; } = 1;
    public int RoundsLeft { get; set; }

    public int GetRoundsLeft() => RoundsLeft;

    public void DecrementRoundsLeft()
    {
        if (RoundsLeft > 0) RoundsLeft--;
    }
}

