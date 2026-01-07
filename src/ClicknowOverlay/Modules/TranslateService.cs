namespace ClicknowOverlay.Modules;

public interface ITranslateService
{
    Task<TranslationResult> TranslateAsync(string text, string primaryLanguage, string secondaryLanguage, CancellationToken cancellationToken);
}

public sealed record TranslationResult(string SourceLanguage, string TargetLanguage, string TranslatedText);
