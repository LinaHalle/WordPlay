namespace Brainfart.Models;

public class CreateGameRequest
{
  public string HostName { get; set; } = string.Empty;
  public List<string> Categories { get; set; } = new();
  public int Rounds { get; set; }
}