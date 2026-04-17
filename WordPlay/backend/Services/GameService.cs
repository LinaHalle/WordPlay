using System.Collections.Concurrent;
using Brainfart.Models;

namespace Brainfart.Services;

public class GameService
{
  private readonly ConcurrentDictionary<Guid, GameState> _games = new();

  public (Guid gameId, Guid playerId) CreateGame(string hostName, List<string> categories, int rounds)
  {
    var gameId = Guid.NewGuid();
    var hostId = Guid.NewGuid();

    var state = new GameState
    {
      GameId = gameId,
      HostId = hostId,
      Status = GameStatus.WaitingForPlayers,
      Categories = categories,
      Rounds = rounds,
      Players = new List<Player> { new Player(hostId, hostName) }
    };
    _games[gameId] = state;
    return (gameId, hostId);
  }

  public (bool found, Guid? playerId, string? error) JoinGame(Guid gameId, string playerName)
  {
    if (!_games.TryGetValue(gameId, out var state))
      return (false, null, null);
    if (state.Status != GameStatus.WaitingForPlayers)
      return (true, null, "Game already started");
    var playerId = Guid.NewGuid();
    state.Players.Add(new Player(playerId, playerName));
    return (true, playerId, null);
  }

  public (bool found, string? letter, string? error) StartGame(Guid gameId)
  {
    if (!_games.TryGetValue(gameId, out var state))
      return (false, null, null);
    if (state.Players.Count < 2)
      return (true, null, "Need at least 2 players");
    state.CurrentRound = 0;
    state.Scoreboard = new Dictionary<Guid, int>();

    StartRound(state);

    return (true, state.CurrentLetter, null);
  }

  private void StartRound(GameState state)
  {
    state.Status = GameStatus.InRound;
    state.RoundStartedAt = DateTime.UtcNow;
    state.IsScored = false;

    state.CurrentLetter = LetterGenerator.RandomLetter();

    state.Answers = new Dictionary<Guid, Dictionary<string, string>>();
    state.SubmittedPlayers = new HashSet<Guid>();
  }

  public (bool found, string? letter, string? error, bool gameFinished) NextRound(Guid gameId)
  {
    if (!_games.TryGetValue(gameId, out var state))
      return (false, null, null, false);

    if (!state.IsScored)
      return (true, null, "Round not scored yet", false);

    state.CurrentRound++;

    if (state.CurrentRound >= state.Rounds)
    {
      state.Status = GameStatus.Finished;
      return (true, null, null, true);
    }

    StartRound(state);

    return (true, state.CurrentLetter, null, false);
  }
  public (bool found, string? error, bool roundFinished) SubmitAnswers(Guid gameId, SubmitAnswersRequest req)
  {
    if (!_games.TryGetValue(gameId, out var state))
      return (false, "Not found", false);

    if (state.Status != GameStatus.InRound)
      return (true, "Round not active", false);

    state.RoundStartedAt ??= DateTime.UtcNow;

    state.Answers[req.PlayerId] = req.Answers;

    state.SubmittedPlayers ??= new HashSet<Guid>();
    state.SubmittedPlayers.Add(req.PlayerId);

    TryFinishRound(state);
    return (true, null, state.Status == GameStatus.RoundFinished);
  }

  private void TryFinishRound(GameState state)
  {
    if (state.Status != GameStatus.InRound)
      return;

    var elapsed = DateTime.UtcNow - state.RoundStartedAt;

    if (elapsed >= TimeSpan.FromSeconds(5) ||
        state.SubmittedPlayers.Count == state.Players.Count)
    {
      state.Status = GameStatus.RoundFinished;
    }
  }

  public (bool found, RoundResult? result, string? error) FinishRound(Guid gameId)
  {
    if (!_games.TryGetValue(gameId, out var state))
      return (false, null, null);

    if (state.Status != GameStatus.RoundFinished)
      return (true, null, "Round not finished yet");

    if (!state.IsScored)
    {
      var result = Scoring.Calculate(state);
      state.Scoreboard = result.Scoreboard;
      state.IsScored = true;
    }
    Console.WriteLine($"SCORING RUNNING - players: {state.Players.Count}");
    Console.WriteLine($"answers: {state.Answers.Count}");
    return (true, new RoundResult(state.Scoreboard), null);
  }

  public (bool found, GameState? state) GetGameState(Guid gameId)
  {
    if (!_games.TryGetValue(gameId, out var state))
      return (false, null);
    return (true, state);
  }
}
