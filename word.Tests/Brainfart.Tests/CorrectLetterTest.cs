namespace Brainfart.Tests;

using Brainfart.Models;
using Brainfart.Services;

public class CreateGameTest
{
  [Fact]
  public void scoringTest()
  {
    var player1Id = Guid.NewGuid();
    var player2Id = Guid.NewGuid();

    var state = new GameState
    {
      CurrentLetter = "A",
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
            {"Cities", "Alabama"},
            {"Fruit", "Apple"}
          }
        },
        {
          player2Id, new Dictionary<string, string>
          {
            { "Cities", "Aloha"},
            {"Fruit", "Alakazam"}
          }
        }
      }
    };

    //act
    var result = Scoring.Calculate(state);

    Assert.Equal(40, result.Scoreboard[player1Id]);
    Assert.Equal(0, result.Scoreboard[player2Id]);


  }
}


