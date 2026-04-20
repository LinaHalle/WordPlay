using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Brainfart.Models;

namespace Brainfart.Services;

public class GameService
{
    private readonly ConcurrentDictionary<Guid, GameState> _games = new();
    private static readonly Regex ValidName = new(@"^[a-zA-Z0-9 ]+$");

    private static string? ValidatePlayerName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "Playername is requiered";
        if (name.Length > 20)
            return "Name must be 20 characters or less";
        if (!ValidName.IsMatch(name))
            return "Name can only contain letters, numbers, and spaces";
        return null;
    }

    public (Guid gameId, Guid playerId, string? error) CreateGame(string hostName)
    {
        
        var gameId = Guid.NewGuid();
        var hostId = Guid.NewGuid();

        // Detta testats i UNIT tester
        var state = new GameState
        {
            GameId = gameId,
            Status = GameStatus.WaitingForPlayers,
            Categories = new List<string>(),
            Players = new List<Player> { new Player(hostId, hostName, true) }
        };
        var nameError = ValidatePlayerName(hostName);
        if (nameError != null)
            return (gameId, hostId, nameError);
        _games[gameId] = state;
        
        // Detta testas i API testerna
        return (gameId, hostId, null);
    }

    public (bool found, string? error) ChooseSettings(Guid gameId, ChooseSettingsRequest req)
    {
        if (!_games.TryGetValue(gameId, out var state))
            return (false, null);
        if (state.Status != GameStatus.WaitingForPlayers)
            return (true, "Game already started");
        if (req.Categories == null || req.Categories.Count == 0)
            return (true, "Categories are requiered.");
        if (req.Rounds <= 0)
            return (true, "Rounds must be at least one");
        state.Categories = req.Categories;
        state.Rounds = req.Rounds;
        state.RoundsLeft = req.Rounds;
        state.Language = req.Language;
        return (true, null);

    }

    public (bool found, Guid? playerId, string? error) JoinGame(Guid gameId, string playerName)
    {
        if (!_games.TryGetValue(gameId, out var state))
            return (false, null, null);
        if (state.Status != GameStatus.WaitingForPlayers)
            return (true, null, "Game already started");
        var nameError = ValidatePlayerName(playerName);
        if (nameError != null)
            return (true, null, nameError);
        var playerId = Guid.NewGuid();
        state.Players.Add(new Player(playerId, playerName, false));
        return (true, playerId, null);
    }

    public (bool found, string? letter, string? error) StartGame(Guid gameId, Guid? playerId)
    {
        if (!_games.TryGetValue(gameId, out var state))
            return (false, null, null);
        if (state.Status != GameStatus.WaitingForPlayers && state.Status != GameStatus.ShowingLeaderboard)
            return (true, null, "Game is not in a startable state");
        if (state.Players.Count < 2)
            return (true, null, "Need at least 2 players");
        if (state.Categories.Count == 0)
            return (true, null, "Categories must be set before starting");
        var player = state.Players.Find(p => p.PlayerId == playerId);
        if (player == null || !player.Host)
            return (true, null, "Only Host can start the game");
        state.Status = GameStatus.InRound;
        state.CurrentLetter = LetterGenerator.RandomLetter();
        state.Answers = new Dictionary<Guid, Dictionary<string, string>>();
        return (true, state.CurrentLetter, null);
    }

    public (bool found, string? error, bool roundFinished) SubmitAnswers(Guid gameId, SubmitAnswersRequest req)
    {
        if (!_games.TryGetValue(gameId, out var state))
            return (false, "Not found", false);
        if (state.Status != GameStatus.InRound && state.Status != GameStatus.WaitingForAnswers)
            return (true, "Round not active", false);
        if (!state.Players.Any(p => p.PlayerId == req.PlayerId))
            return (true, "Player not found in this game", false);
        if (state.Answers.ContainsKey(req.PlayerId))
            return (true, "Answers has already been submitted", false);
        var extraCategories = req.Answers.Keys.Except(state.Categories).ToList();
        if (extraCategories.Any())
            return (true, "Answers contain invalid categories", false);
        state.Answers[req.PlayerId] = req.Answers;
        state.Status = GameStatus.WaitingForAnswers;
        if (state.Answers.Count == state.Players.Count)
            state.Status = GameStatus.RoundFinished;
        return (true, null, state.Status == GameStatus.RoundFinished);
    }

    public (bool found, RoundResult? result, string? error) FinishRound(Guid gameId, CategoryService categoryService)
    {
        if (!_games.TryGetValue(gameId, out var state))
            return (false, null, null);
        if (state.Status != GameStatus.RoundFinished)
            return (true, null, "Round not finished yet");
        var roundResult = Scoring.Calculate(state, categoryService);
        state.Scoreboard = roundResult.Scoreboard;
        state.DecrementRoundsLeft();
        if (state.GetRoundsLeft() > 0)
            state.Status = GameStatus.ShowingLeaderboard;
        if (state.GetRoundsLeft() == 0)
            state.Status = GameStatus.GameEnded;
        return (true, roundResult, null);
    }

    public (bool found, GameState? state) GetGameState(Guid gameId)
    {
        if (!_games.TryGetValue(gameId, out var state))
            return (false, null);
        return (true, state);
    }
}
