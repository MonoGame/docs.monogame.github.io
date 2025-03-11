#region declaration
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Input;

public class InputManager : GameComponent { }
#endregion
{
    #region properties
    /// <summary>
    /// Gets the state information of keyboard input.
    /// </summary>
    public KeyboardInfo Keyboard { get; private set; }

    /// <summary>
    /// Gets the state information of mouse input.
    /// </summary>
    public MouseInfo Mouse { get; private set; }

    /// <summary>
    /// Gets the state information of a gamepad.
    /// </summary>
    public GamePadInfo[] GamePads { get; private set; }
    #endregion

    #region ctors
    /// <summary>
    /// Creates a new InputManager.
    /// </summary>
    /// <param name="game">The game this input manager belongs to.</param>
    public InputManager(Game game) : base(game) { }
    #endregion

    #region methods_initialize
    /// <summary>
    /// Initializes this input manager.
    /// </summary>
    public override void Initialize()
    {
        Keyboard = new KeyboardInfo();
        Mouse = new MouseInfo();

        GamePads = new GamePadInfo[4];
        for (int i = 0; i < 4; i++)
        {
            GamePads[i] = new GamePadInfo((PlayerIndex)i);
        }
    }
    #endregion

    #region methods_update
    /// <summary>
    /// Updates the state information for the keyboard, mouse, and gamepad inputs.
    /// </summary>
    /// <param name="gameTime">A snapshot of the timing values for the current frame.</param>
    public override void Update(GameTime gameTime)
    {
        Keyboard.Update();
        Mouse.Update();

        for (int i = 0; i < 4; i++)
        {
            GamePads[i].Update(gameTime);
        }
    }
    #endregion
}
