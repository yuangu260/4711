namespace ClicknowOverlay.Modules;

public interface IConfigStore
{
    Task<AppConfig> LoadAsync();
    Task SaveAsync(AppConfig config);
}

public sealed record AppConfig(
    bool EnableAutoTrigger,
    string Hotkey,
    string PrimaryLanguage,
    string SecondaryLanguage,
    double ResultFontSize,
    bool KeepOnTop,
    bool EnableStreaming,
    bool EnableWebSearch
);
