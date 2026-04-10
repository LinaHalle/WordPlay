using System.Collections.Concurrent;
using Brainfart.Models;

namespace Brainfart.Services;

public class GameService
{
    private readonly ConcurrentDictionary<Guid, GameState> _games = new();

    public (Guid gameId, Guid playerId, string? error) CreateGame(string hostName)
    {
        var gameId = Guid.NewGuid();
        var hostId = Guid.NewGuid();
        var state = new GameState
        {
            GameId = gameId,
            Status = GameStatus.WaitingForPlayers,
            Categories = new List<string>(),
            Players = new List<Player> { new Player(hostId, hostName, true) }
        };
        if (string.IsNullOrWhiteSpace(hostName))
            return (gameId, hostId, "Playername is requiered");
        _games[gameId] = state;
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
        return (true, null);

    }

    public (bool found, Guid? playerId, string? error) JoinGame(Guid gameId, string playerName)
    {
        if (!_games.TryGetValue(gameId, out var state))
            return (false, null, null);
        if (state.Status != GameStatus.WaitingForPlayers)
            return (true, null, "Game already started");
        if (string.IsNullOrWhiteSpace(playerName))
            return (true, null, "Playername is requiered");
        var playerId = Guid.NewGuid();
        state.Players.Add(new Player(playerId, playerName, false));
        return (true, playerId, null);
    }

    public (bool found, string? letter, string? error) StartGame(Guid gameId, Guid? playerId)
    {
        if (!_games.TryGetValue(gameId, out var state))
            return (false, null, null);
        if (state.Players.Count < 2)
            return (true, null, "Need at least 2 players");
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
        if (state.Status != GameStatus.InRound)
            return (true, "Round not active", false);
        if (state.Answers.ContainsKey(req.PlayerId))
            return (true, "Answers has already been submitted", false);
        state.Answers[req.PlayerId] = req.Answers;
        if (state.Answers.Count == state.Players.Count)
            state.Status = GameStatus.RoundFinished;
        return (true, null, state.Status == GameStatus.RoundFinished);
    }

    public (bool found, RoundResult? result, string? error) FinishRound(Guid gameId)
    {
        if (!_games.TryGetValue(gameId, out var state))
            return (false, null, null);
        if (state.Status != GameStatus.RoundFinished)
            return (true, null, "Round not finished yet");
        var roundResult = Scoring.Calculate(state);
        state.Scoreboard = roundResult.Scoreboard;
        return (true, roundResult, null);
    }

    public (bool found, GameState? state) GetGameState(Guid gameId)
    {
        if (!_games.TryGetValue(gameId, out var state))
            return (false, null);
        return (true, state);
    }
}
