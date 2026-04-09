using System.Collections.Concurrent;
using Brainfart.Models;

namespace Brainfart.Services;

public class GameService
{
    private readonly ConcurrentDictionary<Guid, GameState> _games = new();

    public (Guid gameId, Guid playerId) CreateGame(string hostName)
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
        _games[gameId] = state;
        return (gameId, hostId);
    }

    public (bool found, string? error) ChooseSettings(Guid gameId, List<string> categories, int rounds)
    {
        if (!_games.TryGetValue(gameId, out var state))
            return (false, null);
        if (state.Status != GameStatus.WaitingForPlayers)
            return (true, "Game already started");
        state.Categories = categories;
        state.Rounds = rounds;
        return (true, null);

    }

    public (bool found, Guid? playerId, string? error) JoinGame(Guid gameId, string playerName)
    {
        if (!_games.TryGetValue(gameId, out var state))
            return (false, null, null);
        if (state.Status != GameStatus.WaitingForPlayers)
            return (true, null, "Game already started");
        var playerId = Guid.NewGuid();
        state.Players.Add(new Player(playerId, playerName, false));
        return (true, playerId, null);
    }

    public (bool found, string? letter, string? error) StartGame(Guid gameId)
    {
        if (!_games.TryGetValue(gameId, out var state))
            return (false, null, null);
        if (state.Players.Count < 2)
            return (true, null, "Need at least 2 players");
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
