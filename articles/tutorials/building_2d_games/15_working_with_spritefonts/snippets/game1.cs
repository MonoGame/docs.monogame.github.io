using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Audio;
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

    // Tracks the position of the bat.
    private Vector2 _batPosition;

    // Tracks the velocity of the bat.
    private Vector2 _batVelocity;

    // Tracks the audio manager
    private AudioManager _audio;

    // The SpriteFont Description used to draw text
    private SpriteFont _font;

    // Tracks the players score.
    private int _score;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Create the instance of the input manager.
        _input = new InputManager(this);

        // Add it to the game's component collection
        Components.Add(_input);

        // Create the instance of the audio manager.
        _audio = new AudioManager(this);

        // Add it to the game's component collection
        Components.Add(_audio);
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();

        // Set the initial position of the bat to be 10px
        // to the right of the slime.
        _batPosition = new Vector2(_slime.Width + 10, 0);

        // Assign the initial random velocity to the bat.
        AssignRandomBatVelocity();

        // Start playing the background music
        _audio.PlaySong("audio/theme");
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

        // Load the bounce sound effect
        _audio.AddSoundEffect("audio/bounce");

        // Load the collect sound effect
        _audio.AddSoundEffect("audio/collect");

        // Load the theme song
        _audio.AddSong("audio/theme");

        // Load the font
        _font = Content.Load<SpriteFont>("fonts/gameFont");
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

        // Create a bounding rectangle for the screen
        Rectangle screenBounds = new Rectangle(
            0,
            0,
            GraphicsDevice.PresentationParameters.BackBufferWidth,
            GraphicsDevice.PresentationParameters.BackBufferHeight
        );

        // Creating a bounding circle for the slime
        Circle slimeBounds = new Circle(
            (int)(_slimePosition.X + (_slime.Width * 0.5f)),
            (int)(_slimePosition.Y + (_slime.Height * 0.5f)),
            (int)(_slime.Width * 0.5f)
        );

        // Use distance based checks to determine if the slime is within the
        // bounds of the game screen, and if it's outside that screen edge,
        // move it back inside.
        if (slimeBounds.Left < screenBounds.Left)
        {
            _slimePosition.X = screenBounds.Left;
        }
        else if (slimeBounds.Right > screenBounds.Right)
        {
            _slimePosition.X = screenBounds.Right - _slime.Width;
        }

        if (slimeBounds.Top < screenBounds.Top)
        {
            _slimePosition.Y = screenBounds.Top;
        }
        else if (slimeBounds.Bottom > screenBounds.Bottom)
        {
            _slimePosition.Y = screenBounds.Bottom - _slime.Height;
        }

        // Calculate the new position of the bat based on the velocity
        Vector2 newBatPosition = _batPosition + _batVelocity;

        // Create a bounding circle for the bat
        Circle batBounds = new Circle(
            (int)(newBatPosition.X + (_bat.Width * 0.5f)),
            (int)(newBatPosition.Y + (_bat.Height * 0.5f)),
            (int)(_bat.Width * 0.5f)
        );

        Vector2 normal = Vector2.Zero;

        // Use distance based checks to determine if the bat is within the
        // bounds of the game screen, and if it's outside that screen edge,
        // reflect it about the screen edge normal
        if (batBounds.Left < screenBounds.Left)
        {
            normal.X = Vector2.UnitX.X;
            newBatPosition.X = screenBounds.Left;
        }
        else if (batBounds.Right > screenBounds.Right)
        {
            normal.X = -Vector2.UnitX.X;
            newBatPosition.X = screenBounds.Right - _bat.Width;
        }

        if (batBounds.Top < screenBounds.Top)
        {
            normal.Y = Vector2.UnitY.Y;
            newBatPosition.Y = screenBounds.Top;
        }
        else if (batBounds.Bottom > screenBounds.Bottom)
        {
            normal.Y = -Vector2.UnitY.Y;
            newBatPosition.Y = screenBounds.Bottom - _bat.Height;
        }

        // If the normal is anything but Vector2.Zero, this means the bat had
        // moved outside the screen edge so we should reflect it about the
        // normal.
        if (normal != Vector2.Zero)
        {
            _batVelocity = Vector2.Reflect(_batVelocity, normal);

            // Play bounce sound
            _audio.PlaySoundEffect("audio/bounce");
        }

        _batPosition = newBatPosition;

        if (slimeBounds.Intersects(batBounds))
        {
            // Divide the width  and height of the screen into equal columns and
            // rows based on the width and height of the bat.
            int totalColumns = GraphicsDevice.PresentationParameters.BackBufferWidth / (int)_bat.Width;
            int totalRows = GraphicsDevice.PresentationParameters.BackBufferHeight / (int)_bat.Height;

            // Choose a random row and column based on the total number of each
            int column = Random.Shared.Next(0, totalColumns);
            int row = Random.Shared.Next(0, totalRows);

            // Change the bat position by setting the x and y values equal to
            // the column and row multiplied by the width and height.
            _batPosition = new Vector2(column * _bat.Width, row * _bat.Height);

            // Assign a new random velocity to the bat
            AssignRandomBatVelocity();

            // Play collect sound
            _audio.PlaySoundEffect("audio/collect");

             // Increase the player's score.
            _score += 100;
        }
    }

    private void AssignRandomBatVelocity()
    {
        // Generate a random angle
        float angle = (float)(Random.Shared.NextDouble() * Math.PI * 2);

        // Convert angle to a direction vector
        float x = (float)Math.Cos(angle);
        float y = (float)Math.Sin(angle);
        Vector2 direction = new Vector2(x, y);

        // Multiply the direction vector by the movement speed
        _batVelocity = direction * MOVEMENT_SPEED;
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

        // If the M key is pressed, toggle mute state for audio
        if(_input.Keyboard.WasKeyJustPressed(Keys.M))
        {
            _audio.ToggleMute();
        }

        // If the + button is pressed, increase the volume
        if(_input.Keyboard.WasKeyJustPressed(Keys.OemPlus))
        {
            _audio.IncreaseVolume(0.1f);
        }

        // If the - key is pressed, decrease the volume
        if(_input.Keyboard.WasKeyJustPressed(Keys.OemMinus))
        {
            _audio.DecreaseVolume(0.1f);
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
        if (gamePadOne.LeftThumbStick != Vector2.Zero)
        {
            _slimePosition.X += gamePadOne.LeftThumbStick.X * speed;
            _slimePosition.Y -= gamePadOne.LeftThumbStick.Y * speed;
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

        // Draw the bat sprite.
        _bat.Draw(_spriteBatch, _batPosition);

        // Draw the score
        _spriteBatch.DrawString(_font, $"Score: {_score}", Vector2.Zero, Color.White);

        // Always end the sprite batch when finished.
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
