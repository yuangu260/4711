namespace ClicknowOverlay.Modules;

public interface ISelectionProvider
{
    event EventHandler<SelectionChangedEventArgs> SelectionChanged;
    void EnableAutoTrigger(bool enabled);
    void RegisterHotkey(string hotkey);
}

public sealed class SelectionChangedEventArgs : EventArgs
{
    public SelectionChangedEventArgs(string selectionText, SelectionTrigger trigger)
    {
        SelectionText = selectionText;
        Trigger = trigger;
    }

    public string SelectionText { get; }
    public SelectionTrigger Trigger { get; }
}

public enum SelectionTrigger
{
    Auto,
    Hotkey
}
