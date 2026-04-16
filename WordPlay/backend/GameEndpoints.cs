using Brainfart.Models;
using Brainfart.Services;

namespace Brainfart;

public static class GameEndpoints
{
  public static void MapGameEndpoints(this WebApplication app)
  {
    var gameService = new GameService();

    app.MapPost("/games", (string hostName) =>
    {
      var (gameId, playerId, error) = gameService.CreateGame(hostName);
      if (error != null) return Results.BadRequest(error);
      var state = gameService.GetGameState(gameId).state;
      return Results.Created($"/games/{gameId}", new { gameId, playerId, status = state?.Status.ToString() });
    });

    app.MapPost("/games/{gameId:guid}/settings", (Guid gameId, ChooseSettingsRequest req) =>
    {
      var (found, error) = gameService.ChooseSettings(gameId, req);
      if (!found) return Results.NotFound();
      if (error != null) return Results.BadRequest(error);
      var state = gameService.GetGameState(gameId).state;
      return Results.Ok(new { gameId, req, status = state?.Status.ToString() });
    });

    app.MapPost("/games/{gameId:guid}/join", (Guid gameId, string playerName) =>
    {
      var (found, playerId, error) = gameService.JoinGame(gameId, playerName);
      if (!found) return Results.NotFound();
      if (error != null) return Results.BadRequest(error);
      var state = gameService.GetGameState(gameId).state;
      return Results.Ok(new { gameId, playerId, status = state?.Status.ToString() });
    });

    app.MapPost("/games/{gameId:guid}/start", (Guid gameId, Guid playerId) =>
    {
      var (found, letter, error) = gameService.StartGame(gameId, playerId);
      if (!found) return Results.NotFound();
      if (error != null) return Results.BadRequest(error);
      var state = gameService.GetGameState(gameId).state;
      return Results.Ok(new { letter, status = state?.Status.ToString() });
    });

    app.MapPost("/games/{gameId:guid}/answers", (Guid gameId, SubmitAnswersRequest req) =>
    {
      var (found, error, roundFinished) = gameService.SubmitAnswers(gameId, req);
      if (!found) return Results.NotFound();
      if (error != null) return Results.BadRequest(error);
      var state = gameService.GetGameState(gameId).state;
      return Results.Ok(new { roundFinished, status = state?.Status.ToString() });
    });

    app.MapPost("/games/{gameId:guid}/finish-round", (Guid gameId) =>
    {
      var (found, result, error) = gameService.FinishRound(gameId);
      if (!found) return Results.NotFound();
      if (error != null) return Results.BadRequest(error);
      return Results.Ok(result);
    });

    app.MapGet("/games/{gameId:guid}", (Guid gameId) =>
    {
      var (found, state) = gameService.GetGameState(gameId);
      if (!found) return Results.NotFound();
      return Results.Ok(new
      {
        state!.GameId,
        Status = state.Status.ToString(),
        state.Players,
        state.Categories,
        state.CurrentLetter,
        state.Answers,
        state.Scoreboard,
        state.Rounds,
        state.RoundsLeft
      });
    });
  }
}
