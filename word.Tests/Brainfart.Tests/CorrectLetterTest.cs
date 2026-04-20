namespace Brainfart.Tests;

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
            { "Cities", "Alaska"},
            {"Fruit", "Ananas"}
          }
        }
      }
    };



  }
}


