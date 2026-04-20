using System.Text.Json;

namespace Brainfart.Services;

public class CategoryService
{
    private readonly Dictionary<string, Dictionary<string, HashSet<string>>> _data;

    public CategoryService()
    {
        var json = File.ReadAllText("public/categories.json");
        var raw = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, List<string>>>>(json)!;
        _data = raw.ToDictionary(
            lang => lang.Key,
            lang => lang.Value.ToDictionary(
                cat => cat.Key,
                cat => new HashSet<string>(cat.Value, StringComparer.OrdinalIgnoreCase)));
    }

    public List<string> GetCategoryNames(string language)
    {
        if (!_data.TryGetValue(language, out var categories))
            return new List<string>();
        return categories.Keys.ToList();
    }

    public bool IsValidAnswer(string language, string category, string answer)
    {
        if (string.IsNullOrWhiteSpace(answer))
            return false;
        if (!_data.TryGetValue(language, out var categories))
            return true;
        if (!categories.TryGetValue(category, out var validWords))
            return true;
        return validWords.Contains(answer.Trim());
    }
}
