using Brainfart.Models;
using Brainfart.Services;

namespace Brainfart;

public static class GameEndpoints
{
  public static void MapGameEndpoints(this WebApplication app, GameService gameService)
  {

    app.MapPost("/games", (CreateGameRequest req) =>
    {
      var (gameId, playerId) = gameService.CreateGame(
        req.HostName,
        req.Categories,
        req.Rounds
      );
      return Results.Created($"/games/{gameId}", new { gameId, playerId });
    });

    app.MapPost("/games/{gameId:guid}/join", (Guid gameId, JoinGameRequest req) =>
    {
      var (found, playerId, error) = gameService.JoinGame(gameId, req.Name);
      if (!found) return Results.NotFound();
      if (error != null) return Results.BadRequest(error);
      return Results.Ok(new { gameId, playerId });
    });

    app.MapPost("/games/{gameId:guid}/start", (Guid gameId) =>
    {
      var (found, letter, error) = gameService.StartGame(gameId);
      if (!found) return Results.NotFound();
      if (error != null) return Results.BadRequest(error);
      return Results.Ok(new { letter });
    });

    app.MapPost("/games/{gameId:guid}/answers", (Guid gameId, SubmitAnswersRequest req) =>
    {
      var (found, error, roundFinished) = gameService.SubmitAnswers(gameId, req);
      if (!found) return Results.NotFound();
      if (error != null) return Results.BadRequest(error);
      return Results.Ok();
    });

    app.MapPost("/games/{gameId:guid}/next-round", (Guid gameId) =>
    {
      var (found, letter, error, finished) = gameService.NextRound(gameId);
      if (!found) return Results.NotFound();
      if (error != null) return Results.BadRequest(error);
      return Results.Ok(new { letter, finished });
    });

    app.MapPost("/games/{gameId:guid}/finish-round", (Guid gameId) =>
    {
      var (found, result, error) = gameService.FinishRound(gameId);
      if (!found) return Results.NotFound();
      if (error != null) return Results.BadRequest(error);
      return Results.Ok(result);
    });

    app.MapPost("/games/{gameId:guid}/restart", (Guid gameId) =>
    {
      var (found, error) = gameService.RestartGame(gameId);
      if (!found) return Results.NotFound();
      if (error != null) return Results.BadRequest(error);
      return Results.Ok();
    });

    app.MapGet("/games/{gameId:guid}", (Guid gameId) =>
    {
      var (found, state) = gameService.GetGameState(gameId);
      if (!found) return Results.NotFound();

      return Results.Ok(state);
    });
  }
}
