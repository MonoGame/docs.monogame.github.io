using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;

namespace DungeonSlime;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Defines the slime animated sprite.
    private AnimatedSprite _slime;

    // Defines the bat animated sprite.
    private AnimatedSprite _bat;

    // Tracks the position of the slime.
    private Vector2 _slimePosition;

    // Speed multiplier when moving.
    private const float MOVEMENT_SPEED = 5.0f;

    // Tracks the input manager
    private InputManager _input;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Create the instance of the input manager.
        _input = new InputManager(this);

        // Add it to the game's component collection
        Components.Add(_input);
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Create the texture atlas from the XML configuration file
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");

        // Create the slime animated sprite from the atlas.
        _slime = atlas.CreateAnimatedSprite("slime-animation");

        // Create the bat animated sprite from the atlas.
        _bat = atlas.CreateAnimatedSprite("bat-animation");
    }

    protected override void Update(GameTime gameTime)
    {
        // base.Update first so that components are updated before we perform
        // additional game logic
        base.Update(gameTime);

        // Update the slime animated sprite.
        _slime.Update(gameTime);

        // Update the bat animated sprite.
        _bat.Update(gameTime);

        // Check for keyboard input and handle it.
        CheckKeyboardInput();

        // Check for gamepad input and handle it.
        CheckGamePadInput();

    }

    private void CheckKeyboardInput()
    {
        // Exit the game if the escape key is pressed.
        if (_input.Keyboard.IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        // If the space key is held down, the movement speed increases by 1.5
        float speed = MOVEMENT_SPEED;
        if (_input.Keyboard.IsKeyDown(Keys.Space))
        {
            speed *= 1.5f;
        }

        // If the W or Up keys are down, move the slime up on the screen.
        if (_input.Keyboard.IsKeyDown(Keys.W) || _input.Keyboard.IsKeyDown(Keys.Up))
        {
            _slimePosition.Y -= speed;
        }

        // if the S or Down keys are down, move the slime down on the screen.
        if (_input.Keyboard.IsKeyDown(Keys.S) || _input.Keyboard.IsKeyDown(Keys.Down))
        {
            _slimePosition.Y += speed;
        }

        // If the A or Left keys are down, move the slime left on the screen.
        if (_input.Keyboard.IsKeyDown(Keys.A) || _input.Keyboard.IsKeyDown(Keys.Left))
        {
            _slimePosition.X -= speed;
        }

        // If the D or Right keys are down, move the slime right on the screen.
        if (_input.Keyboard.IsKeyDown(Keys.D) || _input.Keyboard.IsKeyDown(Keys.Right))
        {
            _slimePosition.X += speed;
        }
    }

    private void CheckGamePadInput()
    {
        // Get the info for player one's gamepad.
        GamePadInfo gamePadOne = _input.GamePads[(int)PlayerIndex.One];

        // If the A button is held down, the movement speed increases by 1.5
        // and the gamepad vibrates as feedback to the player.
        float speed = MOVEMENT_SPEED;
        if (gamePadOne.IsButtonDown(Buttons.A))
        {
            speed *= 1.5f;
            gamePadOne.SetVibration(1.0f, TimeSpan.FromSeconds(0.5f));
        }
        else
        {
            gamePadOne.StopVibration();
        }

        // Check thumbstick first since it has priority over which gamepad input
        // is movement.  It has priority since the thumbstick values provide a
        // more granular analog value that can be used for movement.
        if (gamePadOne.ThumbSticks.Left != Vector2.Zero)
        {
            _slimePosition.X += gamePadOne.ThumbSticks.Left.X * speed;
            _slimePosition.Y -= gamePadOne.ThumbSticks.Left.Y * speed;
        }
        else
        {
            // If DPadUp is down, move the slime up on the screen.
            if (gamePadOne.IsButtonDown(Buttons.DPadUp))
            {
                _slimePosition.Y -= speed;
            }

            // If DPadDown is down, move the slime down on the screen.
            if (gamePadOne.IsButtonDown(Buttons.DPadDown))
            {
                _slimePosition.Y += speed;
            }

            // If DPapLeft is down, move the slime left on the screen.
            if (gamePadOne.IsButtonDown(Buttons.DPadLeft))
            {
                _slimePosition.X -= speed;
            }

            // If DPadRight is down, move the slime right on the screen.
            if (gamePadOne.IsButtonDown(Buttons.DPadRight))
            {
                _slimePosition.X += speed;
            }
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Begin the sprite batch to prepare for rendering.
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Draw the slime sprite.
        _slime.Draw(_spriteBatch, _slimePosition);

        // Draw the bat sprite 10px to the right of the slime.
        _bat.Draw(_spriteBatch, new Vector2(_slime.Width + 10, 0));

        // Always end the sprite batch when finished.
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}