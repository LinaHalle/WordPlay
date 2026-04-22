namespace Brainfart.Models;

public record ChooseSettingsRequest(List<string> Categories, int Rounds, string Language = "en");
