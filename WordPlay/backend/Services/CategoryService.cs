using System.Collections.Concurrent;
using System.Text.Json;

namespace Brainfart.Services;

public class CategoryService
{
    private readonly string _basePath;
    private readonly Dictionary<string, List<string>> _categoryNames;
    private readonly ConcurrentDictionary<string, HashSet<string>> _wordCache = new();

    public CategoryService() : this("public/categories")
    {
    }

    public CategoryService(string basePath)
    {
        _basePath = basePath;
        _categoryNames = new Dictionary<string, List<string>>();

        foreach (var langDir in Directory.GetDirectories(basePath))
        {
            var language = Path.GetFileName(langDir);
            var indexPath = Path.Combine(langDir, "_index.json");
            if (File.Exists(indexPath))
            {
                var json = File.ReadAllText(indexPath);
                var names = JsonSerializer.Deserialize<List<string>>(json)!;
                _categoryNames[language] = names;
            }
        }
    }

    public CategoryService(Dictionary<string, Dictionary<string, HashSet<string>>> data)
    {
        _basePath = null!;
        _categoryNames = data.ToDictionary(
            lang => lang.Key,
            lang => lang.Value.Keys.ToList());

        foreach (var (language, categories) in data)
        {
            foreach (var (category, words) in categories)
            {
                _wordCache[$"{language}/{category}"] = words;
            }
        }
    }

    public List<string> GetCategoryNames(string language)
    {
        if (!_categoryNames.TryGetValue(language, out var names))
            return new List<string>();
        return new List<string>(names);
    }

    public bool IsValidAnswer(string language, string category, string answer)
    {
        if (string.IsNullOrWhiteSpace(answer))
            return false;

        var words = GetOrLoadCategory(language, category);
        if (words == null)
            return true;

        return words.Contains(answer.Trim());
    }

    private HashSet<string>? GetOrLoadCategory(string language, string category)
    {
        if (!_categoryNames.TryGetValue(language, out var names))
            return null;
        if (!names.Contains(category))
            return null;

        var cacheKey = $"{language}/{category}";
        return _wordCache.GetOrAdd(cacheKey, _ =>
        {
            var filePath = Path.Combine(_basePath, language, $"{category}.json");
            var json = File.ReadAllText(filePath);
            var wordList = JsonSerializer.Deserialize<List<string>>(json)!;
            return new HashSet<string>(wordList, StringComparer.OrdinalIgnoreCase);
        });
    }
}
