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
      var (gameId, playerId) = gameService.CreateGame(hostName);
      return Results.Created($"/games/{gameId}", new { gameId, playerId });
    });

    app.MapPost("/games/{gameId:guid}/settings", (List<string> categories) =>
    {

    });

    app.MapPost("/games/{gameId:guid}/join", (Guid gameId, string playerName) =>
    {
      var (found, playerId, error) = gameService.JoinGame(gameId, playerName);
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
      return Results.Ok(state);
    });
  }
}
