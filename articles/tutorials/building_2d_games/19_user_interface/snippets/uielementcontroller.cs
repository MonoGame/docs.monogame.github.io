using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Input;
using MonoGameLibrary.UI;

namespace DungeonSlime.UI;

/// <summary>
/// UIElement controller that translates keyboard and gamepad input into UI commands.
/// </summary>
public class UIElementController : IUIElementController
{
    private KeyboardInfo _keyboard;
    private GamePadInfo _gamePad;

    /// <summary>
    /// Initializes a new instance of the UIElementController class.
    /// </summary>
    public UIElementController()
    {
        _keyboard = Core.Input.Keyboard;
        _gamePad = Core.Input.GamePads[0];
    }

    /// <summary>
    /// Determines if an upward navigation command has been triggered.
    /// </summary>
    /// <returns>
    /// True if the up arrow key, DPad up, or left thumbstick up was just pressed; otherwise, false.
    /// </returns>
    public bool NavigateUp()
    {
        return _keyboard.WasKeyJustPressed(Keys.Up) ||
               _gamePad.WasButtonJustPressed(Buttons.DPadUp) ||
               _gamePad.WasButtonJustPressed(Buttons.LeftThumbstickUp);
    }

    /// <summary>
    /// Determines if a downward navigation command has been triggered.
    /// </summary>
    /// <returns>
    /// True if the down arrow key, DPad down, or left thumbstick down was just pressed; otherwise, false.
    /// </returns>
    public bool NavigateDown()
    {
        return _keyboard.WasKeyJustPressed(Keys.Down) ||
               _gamePad.WasButtonJustPressed(Buttons.DPadDown) ||
               _gamePad.WasButtonJustPressed(Buttons.LeftThumbstickDown);
    }

    /// <summary>
    /// Determines if a leftward navigation command has been triggered.
    /// </summary>
    /// <returns>
    /// True if the left arrow key, DPad left, or left thumbstick left was just pressed; otherwise, false.
    /// </returns>
    public bool NavigateLeft()
    {
        return _keyboard.WasKeyJustPressed(Keys.Left) ||
               _gamePad.WasButtonJustPressed(Buttons.DPadLeft) ||
               _gamePad.WasButtonJustPressed(Buttons.LeftThumbstickLeft);
    }

    /// <summary>
    /// Determines if a rightward navigation command has been triggered.
    /// </summary>
    /// <returns>
    /// True if the right arrow key, DPad right, or left thumbstick right was just pressed; otherwise, false.
    /// </returns>
    public bool NavigateRight()
    {
        return _keyboard.WasKeyJustPressed(Keys.Right) ||
               _gamePad.WasButtonJustPressed(Buttons.DPadRight) ||
               _gamePad.WasButtonJustPressed(Buttons.LeftThumbstickRight);
    }

    /// <summary>
    /// Determines if a confirm command has been triggered.
    /// </summary>
    /// <returns>
    /// True if the Enter key or gamepad A button was just pressed; otherwise, false.
    /// </returns>
    public bool Confirm()
    {
        return _keyboard.WasKeyJustPressed(Keys.Enter) ||
               _gamePad.WasButtonJustPressed(Buttons.A);
    }

    /// <summary>
    /// Determines if a cancel command has been triggered.
    /// </summary>
    /// <returns>
    /// True if the Escape key or gamepad B button was just pressed; otherwise, false.
    /// </returns>
    public bool Cancel()
    {
        return _keyboard.WasKeyJustPressed(Keys.Escape) ||
               _gamePad.WasButtonJustPressed(Buttons.B);
    }
}