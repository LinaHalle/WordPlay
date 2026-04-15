using Brainfart.Models;
using Brainfart.Services;
using Xunit;

namespace backend.Tests;

public class GameCreationTests
{
  [Fact]
  public void CreateGame_SavesHostRoundsAndCategories()
  {
    var service = new GameService();
    var categories = new List<string> { "Cities", "Animals" };

    var (gameId, playerId) = service.CreateGame("Alice", categories, 3);
    var (found, state) = service.GetGameState(gameId);

    Assert.True(found);
    Assert.NotNull(state);
    Assert.Equal(gameId, state.GameId);
    Assert.Equal(playerId, state.HostId);
    Assert.Equal(GameStatus.WaitingForPlayers, state.Status);
    Assert.Equal(3, state.Rounds);
    Assert.Equal(categories, state.Categories);
  }

  [Fact]
  public void CreateGame_AddsHostAsFirstPlayer()
  {
    var service = new GameService();

    var (gameId, playerId) = service.CreateGame("Alice", new List<string> { "Cities" }, 2);
    var (_, state) = service.GetGameState(gameId);

    Assert.NotNull(state);
    Assert.Single(state.Players);
    Assert.Equal(playerId, state.Players[0].PlayerId);
    Assert.Equal("Alice", state.Players[0].UserName);
  }
}
