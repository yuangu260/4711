namespace ClicknowOverlay.Modules;

public interface IClipboardManager
{
    Task<string?> CaptureSelectionTextAsync(TimeSpan timeout);
    Task RestoreClipboardAsync();
    Task PasteTextAsync(string text);
}
