using Brainfart.Models;
using Brainfart.Services;
using Xunit;

namespace backend.Tests;

public class GameCreationTests
{
  [Fact]
  public void CreateGame_SavesHostRoundsAndCategories() // med detta test kontrollerar vi att spelinställningarna sparas korrekt
  {
    var service = new GameService(); // här skapar vi en ny "spelmotor" som är ett nytt tomt spel
    var categories = new List<string> { "Cities", "Animals" }; // förbereder en lista med kategorier

    var (gameId, playerId) = service.CreateGame("Alice", categories, 3); // nu skapar vi ett spel med Alice, kategorier och antal rundor
    var (found, state) = service.GetGameState(gameId); // sedan frågar vi om ett spel har skapats med ID:t och sedan får vi ett svar ja/nej

    Assert.True(found); // finns spelet?
    Assert.NotNull(state); // spelets info ska inte vara tomt/null
    Assert.Equal(gameId, state.GameId); // spelets ID skall matcha det ID som skapades först
    Assert.Equal(playerId, state.HostId); // Alice skall vara registrerad som host
    Assert.Equal(GameStatus.WaitingForPlayers, state.Status); // nu går spelet i ett läge där vi väntar på fler spelare
    Assert.Equal(3, state.Rounds); // antal rundor ska vara tre som vi angivit ovan
    Assert.Equal(categories, state.Categories); // kategorier ska vara sparade exakt som vi valde
  }

  [Fact]
  public void CreateGame_AddsHostAsFirstPlayer() // med detta test fokuserar enbart på en sak och det är att Alice läggs till automatiskt som spelare när hon skapar spelet
  {
    var service = new GameService();

    var (gameId, playerId) = service.CreateGame("Alice", new List<string> { "Cities" }, 2); // skapar ett spel med Alice som host
    var (_, state) = service.GetGameState(gameId); // hämtar spelets information, "_", i stället för found eftersom vi inte bryr oss om värdet i detta test

    Assert.NotNull(state); // spelets information skall inte vara tomt/null
    Assert.Single(state.Players); // kontrollerar att listan har exakt ett element (en spelare i form av Alice)
    Assert.Equal(playerId, state.Players[0].PlayerId); // första spelarens ID skall matcha Alices ID
    Assert.Equal("Alice", state.Players[0].UserName); // första spelarens namn skall vara Alice.
  }
}
