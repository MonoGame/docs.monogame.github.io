using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Input;

namespace DungeonSlime;

/// <summary>
/// Provides a game-specific input abstraction that maps physical inputs
/// to game actions, bridging our input system with game-specific functionality.
/// </summary>
public class GameController
{
    private KeyboardInfo _keyboard;
    private GamePadInfo _gamePad;

    /// <summary>
    /// Creates a new GameController that handles input for the game.
    /// </summary>
    public GameController()
    {
        _keyboard = Core.Input.Keyboard;
        _gamePad = Core.Input.GamePads[0];
    }

    /// <summary>
    /// Returns true if the player has triggered the "move up" action.
    /// </summary>
    public bool MoveUp()
    {
        return _keyboard.WasKeyJustPressed(Keys.Up) ||
               _keyboard.WasKeyJustPressed(Keys.W) ||
               _gamePad.WasButtonJustPressed(Buttons.DPadUp) ||
               _gamePad.WasButtonJustPressed(Buttons.LeftThumbstickUp);
    }

    /// <summary>
    /// Returns true if the player has triggered the "move down" action.
    /// </summary>
    public bool MoveDown()
    {
        return _keyboard.WasKeyJustPressed(Keys.Down) ||
               _keyboard.WasKeyJustPressed(Keys.S) ||
               _gamePad.WasButtonJustPressed(Buttons.DPadDown) ||
               _gamePad.WasButtonJustPressed(Buttons.LeftThumbstickDown);
    }

    /// <summary>
    /// Returns true if the player has triggered the "move left" action.
    /// </summary>
    public bool MoveLeft()
    {
        return _keyboard.WasKeyJustPressed(Keys.Left) ||
               _keyboard.WasKeyJustPressed(Keys.A) ||
               _gamePad.WasButtonJustPressed(Buttons.DPadLeft) ||
               _gamePad.WasButtonJustPressed(Buttons.LeftThumbstickLeft);
    }

    /// <summary>
    /// Returns true if the player has triggered the "move right" action.
    /// </summary>
    public bool MoveRight()
    {
        return _keyboard.WasKeyJustPressed(Keys.Right) ||
               _keyboard.WasKeyJustPressed(Keys.D) ||
               _gamePad.WasButtonJustPressed(Buttons.DPadRight) ||
               _gamePad.WasButtonJustPressed(Buttons.LeftThumbstickRight);
    }

    /// <summary>
    /// Returns true if the player has triggered the "pause" action.
    /// </summary>
    public bool Pause()
    {
        return _keyboard.WasKeyJustPressed(Keys.Escape) ||
               _gamePad.WasButtonJustPressed(Buttons.Start);
    }

    /// <summary>
    /// Returns true if the player has triggered the "action" button,
    /// typically used for menu confirmation.
    /// </summary>
    public bool Action()
    {
        return _keyboard.WasKeyJustPressed(Keys.Enter) ||
               _gamePad.WasButtonJustPressed(Buttons.A);
    }
}
