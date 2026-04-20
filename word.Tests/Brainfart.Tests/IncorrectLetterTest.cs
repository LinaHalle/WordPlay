namespace Brainfart.Tests;

using Brainfart.Models;
using Brainfart.Services;

public class IncorrectLetterTest
{
  [Fact]
  public void scoringTest_IncorrectLetter()
  {
    var player1Id = Guid.NewGuid();
    var player2Id = Guid.NewGuid();

    var state = new GameState
    {
      CurrentLetter = "B",
      Categories = new List<string> { "Cities", "Fruit" },
      Players = new List<Player>
      {
        new Player(player1Id, "Alice", true),
        new Player(player2Id, "Bob", false)
      },
      Answers = new Dictionary<Guid, Dictionary<string, string>>
      {
        {
          player1Id, new Dictionary<string, string>
          {
            {"Cities", "Amsterdam"},
            {"Fruit", "Apricot"}
          }
        },
        {
          player2Id, new Dictionary<string, string>
          {
            { "Cities", "Alvesta"},
            {"Fruit", "Apple"}
          }
        }
      }
    };

    var result = Scoring.Calculate(state);

    Assert.Equal(0, result.Scoreboard[player1Id]);
    Assert.Equal(0, result.Scoreboard[player2Id]);

  }
}
