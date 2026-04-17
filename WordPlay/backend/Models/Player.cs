namespace Brainfart.Models;

public record Player
(
  Guid PlayerId,
  string Name,
  bool Host = false
);