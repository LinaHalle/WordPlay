using Brainfart.Models;
using Brainfart.Services;

namespace Brainfart;

public static class GameEndpoints
{
  public static void MapGameEndpoints(this WebApplication app, GameService gameService, CategoryService categoryService)
  {

    app.MapGet("/categories/{language}", (string language) =>
    {
      var names = categoryService.GetCategoryNames(language);
      if (names.Count == 0) return Results.NotFound();
      return Results.Ok(names);
    });

    app.MapPost("/games", (CreateGameRequest req) =>
    {
      var (gameId, playerId, error) = gameService.CreateGame(req.HostName);

      if (error != null) return Results.BadRequest(error);

      var state = gameService.GetGameState(gameId).state;

      return Results.Created($"/games/{gameId}", new
      {
        gameId,
        playerId,
        status = state?.Status.ToString()
      });
    });

    app.MapPost("/games/{gameId:guid}/settings", (Guid gameId, ChooseSettingsRequest req) =>
    {
      var (found, error) = gameService.ChooseSettings(gameId, req);
      if (!found) return Results.NotFound();
      if (error != null) return Results.BadRequest(error);
      var state = gameService.GetGameState(gameId).state;
      return Results.Ok(new { gameId, req, status = state?.Status.ToString() });
    });

    app.MapPost("/games/{gameId:guid}/join", (Guid gameId, JoinGameRequest req) =>
    {
      var (found, playerId, error) = gameService.JoinGame(gameId, req.Name);
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
      var (found, error, roundFinished) = gameService.SubmitAnswers(gameId, req, categoryService);
      if (!found) return Results.NotFound();
      if (error != null) return Results.BadRequest(error);
      var state = gameService.GetGameState(gameId).state;
      return Results.Ok(new { roundFinished, status = state?.Status.ToString() });
    });

    // TODO: Uncomment when GameService.NextRound is implemented
    // app.MapPost("/games/{gameId:guid}/next-round", (Guid gameId) =>
    // {
    //   var (found, letter, error, finished) = gameService.NextRound(gameId);
    //   if (!found) return Results.NotFound();
    //   if (error != null) return Results.BadRequest(error);
    //   return Results.Ok(new { letter, finished });
    // });

    app.MapPost("/games/{gameId:guid}/finish-round", (Guid gameId) =>
    {
      var (found, result, error) = gameService.FinishRound(gameId, categoryService);
      if (!found) return Results.NotFound();
      if (error != null) return Results.BadRequest(error);
      return Results.Ok(result);
    });

    // TODO: Uncomment when GameService.RestartGame is implemented
    // app.MapPost("/games/{gameId:guid}/restart", (Guid gameId) =>
    // {
    //   var (found, error) = gameService.RestartGame(gameId);
    //   if (!found) return Results.NotFound();
    //   if (error != null) return Results.BadRequest(error);
    //   return Results.Ok();
    // });

    app.MapGet("/games/{gameId:guid}", (Guid gameId) =>
    {
      var (found, state) = gameService.GetGameState(gameId);
      if (!found) return Results.NotFound();
      return Results.Ok(new
      {
        state!.GameId,
        HostId = state.Players.FirstOrDefault(p => p.Host)?.PlayerId,
        Status = state.Status.ToString(),
        state.Players,
        state.Categories,
        state.CurrentLetter,
        state.Answers,
        state.Scoreboard,
        state.Rounds,
        state.RoundsLeft,
        state.Language
      });
    });
  }
}
