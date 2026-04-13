namespace Brainfart.Models;

public class GameState
{
  public Guid GameId { get; set; }
  public Guid HostId { get; set; }
  public GameStatus Status { get; set; }
  public List<Player> Players { get; set; } = new();
  public List<string> Categories { get; set; } = new();
  public int Rounds { get; set; }
  public string CurrentLetter { get; set; } = "";
  public Dictionary<Guid, Dictionary<string, string>> Answers { get; set; } = new();
  public Dictionary<Guid, int> Scoreboard { get; set; } = new();
}
