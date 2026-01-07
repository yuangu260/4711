namespace ClicknowOverlay.Modules;

public interface IOverlayUi
{
    void ShowMenu(OverlayPosition position, string selectionText);
    void HideMenu();
    void ShowResult(ActionType actionType, string content);
    void HideResult();
}

public sealed record OverlayPosition(double X, double Y, double Width, double Height);

public enum ActionType
{
    Search,
    Summary,
    Translate
}
