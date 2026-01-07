namespace ClicknowOverlay.Modules;

public interface ISearchProvider
{
    Task<IReadOnlyList<SearchResult>> SearchAsync(string query, int topN, CancellationToken cancellationToken);
}

public sealed record SearchResult(string Title, string Snippet, Uri Url);
