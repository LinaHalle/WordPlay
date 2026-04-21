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

    var (gameId, playerId, createError) = service.CreateGame("Alice"); // Nu skapar vi ett spel med Alice
    var (settingsFound, settingsError) = service.ChooseSettings(gameId, new ChooseSettingsRequest(categories, 3)); // Sätter kategorier och antal rundor separat
    var (found, state) = service.GetGameState(gameId); // Sedan frågar vi om ett spel har skapats med ID:t och sedan får vi ett svar ja/nej

    Assert.Null(createError); // Inget fel ska uppstå vid skapandet
    Assert.True(settingsFound); // Spelet ska hittas när inställningar sätts
    Assert.Null(settingsError); // Inget fel ska uppstå när inställningar sätts
    Assert.True(found); // Finns spelet?
    Assert.NotNull(state); // Spelets info ska inte vara tomt/null
    Assert.Equal(gameId, state.GameId); // Spelets ID skall matcha det ID som skapades först
    Assert.Equal(GameStatus.WaitingForPlayers, state.Status); // Nu går spelet i ett läge där vi väntar på fler spelare
    Assert.Equal(3, state.Rounds); // Antal rundor ska vara tre som vi angivit ovan
    Assert.Equal(categories, state.Categories); // Kategorier ska vara sparade exakt som vi valde
    Assert.Contains(state.Players, p => p.PlayerId == playerId && p.Host); // Alice skall vara registrerad som host
  }

  [Fact]
  public void CreateGame_AddsHostAsFirstPlayer() // Detta test fokuserar enbart på en sak och det är att Alice läggs till automatiskt som spelare när hon skapar spelet
  {
    var service = new GameService();

    var (gameId, playerId, error) = service.CreateGame("Alice"); // Skapar ett spel med Alice som host
    var (_, state) = service.GetGameState(gameId); // Hämtar spelets information, "_", i stället för found eftersom vi inte bryr oss om värdet i detta test

    Assert.Null(error); // Skapandet ska lyckas
    Assert.NotNull(state); // Spelets information skall inte vara tomt/null
    Assert.Single(state.Players); // Kontrollerar att listan har exakt ett element (en spelare i form av Alice)
    Assert.Equal(playerId, state.Players[0].PlayerId); // Första spelarens ID skall matcha Alices ID
    Assert.Equal("Alice", state.Players[0].Name); // Första spelarens namn skall vara Alice
  }

  [Fact]
  public void JoinGame_SecondPlayer() // Detta test kontrollerar vi att en andra spelare kan gå med i ett spel som redan skapats
  {
    var service = new GameService(); // Här skapar vi en ny "spelmotor" som är ett nytt tomt spel

    var (gameId, _, createError) = service.CreateGame("Alice"); // Alice skapar ett spel och vi bryr oss enbart om gameId här, därav "_"
    var (found, joinedPlayerId, error) = service.JoinGame(gameId, "Bob");
    // Nu ska Bob försöka gå med i Alice spel med hjälp av gameId
    // Vi ska få tillbaka:
    // 1. Hittades spelet? (found)
    // 2. Bobs unika ID om han kom in (joinedPlayerId)
    // 3. Error, ett felmeddelande om något gick fel (error)
    var (_, state) = service.GetGameState(gameId); // Hämta spelets nuvarande information så vi kan se om Bob är med

    Assert.Null(createError); // Skapandet ska lyckas
    Assert.True(found); // Finns spelet?
    Assert.Null(error); // Inget fel ska ha uppstått (error = null = tomt)
    Assert.NotNull(joinedPlayerId); // Bob ska ha fått ett eget spelar-ID (inte null = tomt)
    Assert.NotNull(state); // Spelets info ska inte vara tomt
    Assert.Equal(2, state.Players.Count); // Nu ska det finnas två spelare i spelet
    Assert.Contains(state.Players, p => p.PlayerId == joinedPlayerId && p.Name == "Bob"); // En kontroll om Bob verkligen finns med i spelarlistan "hitta en spelare där både ID:t matchar Bobs ID och namnet är Bob
  }
  [Fact]
  public void StartGame_StartsRoundAndSetsLetter() // Detta test kontrollerar att spelet startar korrekt efter att en andra spelare gått med
  {
    var service = new GameService();

    var (gameId, hostId, createError) = service.CreateGame("Alice"); // Alice skapar spelet
    service.ChooseSettings(gameId, new ChooseSettingsRequest(new List<string> { "Cities" }, 2)); // Sätter kategorier och rundor innan start
    service.JoinGame(gameId, "Bob"); // Bob går också med så det blir två spelare

    var (found, letter, error) = service.StartGame(gameId, hostId); // Nu startar vi spelet
    var (_, state) = service.GetGameState(gameId); // Hämtar state efter start

    Assert.Null(createError); // Skapandet ska lyckas
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

    var (gameId, hostId, createError) = service.CreateGame("Alice"); // Alice skapar ett spel men ingen annan går med
    service.ChooseSettings(gameId, new ChooseSettingsRequest(new List<string> { "Cities" }, 2)); // Kategorier måste vara satta innan startförsöket

    var (found, letter, error) = service.StartGame(gameId, hostId); // Vi försöker starta spelet trots att det bara finns en spelare
    var (_, state) = service.GetGameState(gameId); // Vi hämtar också spelets info för att kolla att ingenting förändrades

    Assert.Null(createError); // Skapandet ska lyckas
    Assert.True(found); // Spelet hittades
    Assert.Equal("Need at least 2 players", error); // Felmeddelande om Alice vill starta spelet själv (en spelare)
    Assert.Null(letter); // Ingen bokstav ska ha genererats eftersom spelet aldrig startade

    Assert.NotNull(state); // Spelets information skall inte vara tomt
    Assert.Equal(GameStatus.WaitingForPlayers, state.Status); // Spelet ska fortfarande vara i vänteläge
    Assert.Equal("", state.CurrentLetter); // Ingen bokstav ska ha sparats i state heller
    Assert.Empty(state.Answers); // Inga svar ska finnas sparade
  }

  [Fact]
  public void SubmitAnswers_FirstPlayer_SavesAnswersButDoesNotFinishRound() // Testar att första spelarens svar sparas utan att rundan avslutas direkt
  {
    var service = new GameService();

    var (gameId, hostId, createError) = service.CreateGame("Alice"); // Alice skapar ett spel som host
    service.ChooseSettings(gameId, new ChooseSettingsRequest(new List<string> { "Cities", "Animals" }, 2)); // Sätter kategorier och rundor innan start
    var (_, joinedPlayerId, _) = service.JoinGame(gameId, "Bob"); // Bob ansluter

    service.StartGame(gameId, hostId); // Spelet startar och det är dags för att svara

    var request = new SubmitAnswersRequest( // Här bygger vi ihop Alice svar med en request-modell där kategori är nyckel och svaret är värdet, därav dictionary
      hostId,
      new Dictionary<string, string>
      {
        ["Cities"] = "Stockholm",
        ["Animals"] = "Seal"
      }
    );

    var (found, error, roundFinished) = service.SubmitAnswers(gameId, request); // Alice svar skickas in och får tillbaka found, error och om rundan är avslutad
    var (_, state) = service.GetGameState(gameId); // Hämtar spelets nuvarande information för att se vad som sparats

    Assert.Null(createError); // Skapandet ska lyckas
    Assert.True(found); // Spelet ska hittas
    Assert.Null(error); // Inget fel uppkommer
    Assert.False(roundFinished); // Rundan ska INTE vara avslutad eftersom Bob inte har skickat in sina svar än
    Assert.NotNull(state); // Spelets information skall inte vara tomt
    Assert.Equal(GameStatus.WaitingForAnswers, state.Status); // Spelet väntar nu på övriga spelares svar
    Assert.NotNull(state.Answers); // Samlingen av svar skall existera
    Assert.Single(state.Answers); // Det skall enbart finnas ett svar i samlingen, Alices svar
    Assert.Equal("Stockholm", state.Answers[hostId]["Cities"]); // Hennes svar på stad
    Assert.Equal("Seal", state.Answers[hostId]["Animals"]); // Hennes svar på djur
    Assert.NotNull(joinedPlayerId); // Bob ska ha fått ett giltigt spelar-ID
    Assert.False(state.Answers.ContainsKey(joinedPlayerId.Value)); // Bobs ID skall inte existera i samlingen av svar då han inte svarat än. ".Value" används för att komma åt värdet inut i Guid 
  }

  [Theory] // Fungerar som [Fact], dock smartare där vi testar olika värden, flera gånger på ett och samma test

  [InlineData("")] // Testar ett tomt namn
  [InlineData("   ")] // Testar ett namn med enbart mellanslag
  [InlineData("ThisNameIsWayTooLong123")] // Testar ett namn som är för långt
  [InlineData("Alice!")] // Testar ett namn som innehåller otillåtna tecken
  public void CreateGame_InvalidHostName_ReturnsError(string hostName) // "string hostName" tar emot ett InlineData-värde i taget 
  {
    var service = new GameService(); // Försöker skapa en ny "spelmotor" med ogiltigt namn

    var (_, _, error) = service.CreateGame(hostName); // Vi vill enbart ha en output på error

    Assert.NotNull(error);
    // Testet förväntar sig att error INTE är null. Ett felmeddelande skall returneras
    // Om GameService accepterar ett ogiltigt namn utan att klaga så misslyckas testet
  }
}
