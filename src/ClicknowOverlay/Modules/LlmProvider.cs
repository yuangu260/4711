namespace ClicknowOverlay.Modules;

public interface ILlmProvider
{
    Task<string> GenerateSearchSummaryAsync(string query, IReadOnlyList<SearchResult> sources, CancellationToken cancellationToken);
    Task<string> GenerateSummaryAsync(string text, CancellationToken cancellationToken);
    Task<string> TranslateAsync(string text, string fromLanguage, string toLanguage, CancellationToken cancellationToken);
}
