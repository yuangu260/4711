namespace ClicknowOverlay.Modules;

public interface IActionRouter
{
    Task<ActionResult> ExecuteAsync(ActionType actionType, string selectionText, CancellationToken cancellationToken);
}

public sealed record ActionResult(ActionType ActionType, string Content);
