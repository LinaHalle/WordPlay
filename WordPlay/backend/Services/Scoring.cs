using Brainfart.Models;

namespace Brainfart.Services;

public static class Scoring
{
  public static RoundResult Calculate(GameState state)
  {
    var scores = state.Scoreboard != null
     ? new Dictionary<Guid, int>(state.Scoreboard)
     : new Dictionary<Guid, int>();

    foreach (var player in state.Players)
    {
      if (!scores.ContainsKey(player.PlayerId))
        scores[player.PlayerId] = 0;
    }

    foreach (var category in state.Categories)
    {
      var answers = state.Answers.ToDictionary(
        x => x.Key,
         x =>
         {
           var svar = x.Value.GetValueOrDefault(category, "");
           return svar.StartsWith(state.CurrentLetter, StringComparison.OrdinalIgnoreCase) ? svar : "";
         });


      var grouped = answers
          .GroupBy(x => x.Value.Trim().ToLower())
          .ToDictionary(g => g.Key, g => g.Select(x => x.Key).ToList());

      foreach (var group in grouped)
      {
        var word = group.Key;
        var players = group.Value;

        if (string.IsNullOrWhiteSpace(word))
        {
          // Tomt svar -> 0 poäng
          continue;
        }

        if (players.Count == 1)
        {
          // Unikt ord -> 20 poäng
          scores[players[0]] = scores.GetValueOrDefault(players[0]) + 20;
        }
        else if (players.Count > 1)
        {
          // Samma ord -> 5 poäng var
          foreach (var p in players)
            scores[p] = scores.GetValueOrDefault(p) + 5;
        }
      }
    }

    return new RoundResult(scores);
  }
}
