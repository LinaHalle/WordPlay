using Brainfart.Models;
using Brainfart.Services;
using Xunit;

namespace backend.Tests;

public class GameCreationTests
{
  [Fact]
  public void CreateGame_SavesHostRoundsAndCategories() // Detta test kontrollerar vi att spelinställningarna sparas korrekt
  {
    var service = new GameService(); // Här skapar vi en ny "spelmotor" som är ett nytt tomt spel
    var categories = new List<string> { "Cities", "Animals" }; // Förbereder en lista med kategorier

    var (gameId, playerId) = service.CreateGame("Alice", categories, 3); // Nu skapar vi ett spel med Alice, kategorier och antal rundor
    var (found, state) = service.GetGameState(gameId); // Sedan frågar vi om ett spel har skapats med ID:t och sedan får vi ett svar ja/nej

    Assert.True(found); // Finns spelet?
    Assert.NotNull(state); // Spelets info ska inte vara tomt/null
    Assert.Equal(gameId, state.GameId); // Spelets ID skall matcha det ID som skapades först
    Assert.Equal(playerId, state.HostId); // Alice skall vara registrerad som host
    Assert.Equal(GameStatus.WaitingForPlayers, state.Status); // Nu går spelet i ett läge där vi väntar på fler spelare
    Assert.Equal(3, state.Rounds); // Antal rundor ska vara tre som vi angivit ovan
    Assert.Equal(categories, state.Categories); // Kategorier ska vara sparade exakt som vi valde
  }

  [Fact]
  public void CreateGame_AddsHostAsFirstPlayer() // Detta test fokuserar enbart på en sak och det är att Alice läggs till automatiskt som spelare när hon skapar spelet
  {
    var service = new GameService();

    var (gameId, playerId) = service.CreateGame("Alice", new List<string> { "Cities" }, 2); // Skapar ett spel med Alice som host
    var (_, state) = service.GetGameState(gameId); // Hämtar spelets information, "_", i stället för found eftersom vi inte bryr oss om värdet i detta test

    Assert.NotNull(state); // Spelets information skall inte vara tomt/null
    Assert.Single(state.Players); // Kontrollerar att listan har exakt ett element (en spelare i form av Alice)
    Assert.Equal(playerId, state.Players[0].PlayerId); // Första spelarens ID skall matcha Alices ID
    Assert.Equal("Alice", state.Players[0].UserName); // Första spelarens namn skall vara Alice
  }

  [Fact]
  public void JoinGame_SecondPlayer() // Detta test kontrollerar vi att en andra spelare kan gå med i ett spel som redan skapats
  {
    var service = new GameService(); // Här skapar vi en ny "spelmotor" som är ett nytt tomt spel

    var (gameId, _) = service.CreateGame("Alice", new List<string> { "Cities" }, 2); // Alice skapar ett spel och vi bryr oss enbart om gameId här, därav "_"
    var (found, joinedPlayerId, error) = service.JoinGame(gameId, "Bob");
    // Nu ska Bob försöka gå med i Alice spel med hjälp av gameId
    // Vi ska få tillbaka:
    // 1. Hittades spelet? (found)
    // 2. Bobs unika ID om han kom in (joinedPlayerId)
    // 3. Error, ett felmeddelande om något gick fel (error)
    var (_, state) = service.GetGameState(gameId); // Hämta spelets nuvarande information så vi kan se om Bob är med

    Assert.True(found); // Finns spelet?
    Assert.Null(error); // Inget fel ska ha uppstått (error = null = tomt)
    Assert.NotNull(joinedPlayerId); // Bob ska ha fått ett eget spelar-ID (inte null = tomt)
    Assert.NotNull(state); // Spelets info ska inte vara tomt
    Assert.Equal(2, state.Players.Count); // Nu ska det finnas två spelare i spelet
    Assert.Contains(state.Players, p => p.PlayerId == joinedPlayerId && p.UserName == "Bob"); // En kontroll om Bob verkligen finns med i spelarlistan "hitta en spelare där både ID:t matchar Bobs ID och namnet är Bob
  }
  [Fact]
  public void StartGame_StartsRoundAndSetsLetter() // Detta test kontrollerar att spelet startar korrekt efter att en andra spelare gått med
  {
    var service = new GameService();

    var (gameId, _) = service.CreateGame("Alice", new List<string> { "Cities" }, 2); // Alice skapar spelet
    service.JoinGame(gameId, "Bob"); // Bob går också med så det blir två spelare

    var (found, letter, error) = service.StartGame(gameId); // Nu startar vi spelet
    var (_, state) = service.GetGameState(gameId); // Hämtar state efter start

    Assert.True(found); // Spelet ska finnas
    Assert.Null(error); // Inget fel ska uppstå
    Assert.NotNull(letter); // En bokstav ska ha genererats
    Assert.NotNull(state); // Spelets info ska inte vara tomt
    Assert.Equal(GameStatus.InRound, state.Status); // Spelet ska nu vara i en aktiv runda
    Assert.Equal(letter, state.CurrentLetter); // Samma bokstav ska finnas sparad i state
    Assert.NotNull(state.Answers); // En tom samling får spelarnas svar ska ha skapats
    Assert.Empty(state.Answers); // Ingen har skickat in svar än
  }

  // Edge cases-tester
  [Fact]
  public void StartGame_FailsWithOnlyOnePlayer() // Testar att spelet INTE startar om det bara finns en spelare
  {
    var service = new GameService();

    var (gameId, _) = service.CreateGame("Alice", new List<string> { "Cities" }, 2); // Alice skapar ett spel men ingen annan går med

    var (found, letter, error) = service.StartGame(gameId); // Vi försöker starta spelet trots att det bara finns en spelare
    var (_, state) = service.GetGameState(gameId); // Vi hämtar också spelets info för att kolla att ingenting förändrades

    Assert.True(found); // Spelet hittades
    Assert.Equal("Need at least 2 players", error); // Felmeddelande om Alice vill starta spelet själv (en spelare)
    Assert.Null(letter); // Ingen bokstav ska ha genererats eftersom spelet aldrig startade

    Assert.NotNull(state); // Spelets information skall inte vara tomt
    Assert.Equal(GameStatus.WaitingForPlayers, state.Status); // Spelet ska fortfarande vara i vänteläge
    Assert.Null(state.CurrentLetter); // Ingen bokstav ska ha sparats i state heller
    Assert.Null(state.Answers); // Samlingen av svar har inte skapats heller
  }
}
