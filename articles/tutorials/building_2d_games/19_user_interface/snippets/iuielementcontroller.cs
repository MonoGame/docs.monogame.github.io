namespace MonoGameLibrary.UI;

/// <summary>
/// Interface for handling UI element navigation through various input sources.
/// Implement this to create custom controllers for keyboard, gamepad, or other input methods.
/// </summary>
public interface IUIElementController
{
    /// <summary>
    /// Determines if the user has attempted to navigate upward in the UI.
    /// </summary>
    /// <returns>True if upward navigation was triggered; otherwise, false.</returns>
    bool NavigateUp();

    /// <summary>
    /// Determines if the user has attempted to navigate downward in the UI.
    /// </summary>
    /// <returns>True if downward navigation was triggered; otherwise, false.</returns>
    bool NavigateDown();

    /// <summary>
    /// Determines if the user has attempted to navigate left in the UI.
    /// </summary>
    /// <returns>True if leftward navigation was triggered; otherwise, false.</returns>
    bool NavigateLeft();

    /// <summary>
    /// Determines if the user has attempted to navigate right in the UI.
    /// </summary>
    /// <returns>True if rightward navigation was triggered; otherwise, false.</returns>
    bool NavigateRight();

    /// <summary>
    /// Determines if the user has triggered a confirmation action on the current UI element.
    /// </summary>
    /// <returns>True if confirmation was triggered; otherwise, false.</returns>
    bool Confirm();

    /// <summary>
    /// Determines if the user has triggered a cancellation action on the current UI element.
    /// </summary>
    /// <returns>True if cancellation was triggered; otherwise, false.</returns>
    bool Cancel();
}
