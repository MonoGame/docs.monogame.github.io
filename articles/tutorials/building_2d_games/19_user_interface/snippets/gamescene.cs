using System;
using DungeonSlime.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.UI;

namespace DungeonSlime.Scenes;

public class GameScene : Scene
{
    private UISprite _pauseMenu;
    private UISprite _gameOverMenu;

    // Defines the slime animated sprite.
    private AnimatedSprite _slime;

    // Defines the bat animated sprite.
    private AnimatedSprite _bat;

    // Tracks the position of the slime.
    private Vector2 _slimePosition;

    // Speed multiplier when moving.
    private const float MOVEMENT_SPEED = 5.0f;

    // Tracks the position of the bat.
    private Vector2 _batPosition;

    // Tracks the velocity of the bat.
    private Vector2 _batVelocity;

    // Defines the tilemap to draw.
    private Tilemap _tilemap;

    // Defines the bounds of the room that the slime and bat are contained within.
    private Rectangle _roomBounds;

    // The sound effect to play when the bat bounces off the edge of the screen.
    private SoundEffect _bounceSoundEffect;

    // The sound effect to play when the slime eats a bat.
    private SoundEffect _collectSoundEffect;

    // The SpriteFont Description used to draw text
    private SpriteFont _font;

    // Tracks the players score.
    private int _score;

    // Defines the position to draw the score text at.
    private Vector2 _scoreTextPosition;

    // Defines the origin used when drawing the score text.
    private Vector2 _scoreTextOrigin;

    public override void Initialize()
    {
        // LoadContent is called during base.Initialize().
        base.Initialize();

        // During the game scene, we want to disable exit on escape. Instead,
        // the escape key will be used to return back to the title screen
        Core.ExitOnEscape = false;

        Rectangle screenBounds = Core.GraphicsDevice.PresentationParameters.Bounds;

        _roomBounds = new Rectangle(
            _tilemap.TileSize,
            _tilemap.TileSize,
            screenBounds.Width - _tilemap.TileSize * 2,
            screenBounds.Height - _tilemap.TileSize * 2
        );

        // Initial slime position will be the center tile of the tile map.
        int centerRow = _tilemap.Rows / 2;
        int centerColumn = _tilemap.Columns / 2;
        _slimePosition = new Vector2(centerColumn, centerRow) * _tilemap.TileSize;

        // Initial bat position will the in the top left corner of the room
        _batPosition = new Vector2(_roomBounds.Left, _roomBounds.Top);

        // Set the position of the score text to align to the left edge of the
        // room bounds, and to vertically be at the center of the first tile.
        _scoreTextPosition = new Vector2(_roomBounds.Left, _tilemap.TileSize * 0.5f);

        // Set the origin of the text so it's left-centered.
        float scoreTextYOrigin = _font.MeasureString("Score").Y * 0.5f;
        _scoreTextOrigin = new Vector2(0, scoreTextYOrigin);

        // Assign the initial random velocity to the bat.
        AssignRandomBatVelocity();
    }

    public override void LoadContent()
    {
        // Create the texture atlas from the XML configuration file
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/atlas-definition.xml");

        // Create the slime animated sprite from the atlas.
        _slime = atlas.CreateAnimatedSprite("slime-animation");

        // Create the bat animated sprite from the atlas.
        _bat = atlas.CreateAnimatedSprite("bat-animation");

        // Load the tilemap from the XML configuration file.
        _tilemap = Tilemap.FromFile(Content, "images/tilemap-definition.xml");

        // Load the bounce sound effect
        _bounceSoundEffect = Content.Load<SoundEffect>("audio/bounce");

        // Load the collect sound effect
        _collectSoundEffect = Content.Load<SoundEffect>("audio/collect");

        // Load the font
        _font = Core.Content.Load<SpriteFont>("fonts/gameFont");

        // Load the sound effect to play when ui actions occur.
        SoundEffect uiSoundEffect = Core.Content.Load<SoundEffect>("audio/ui");

        // Create the UI Controller
        UIElementController controller = new UIElementController();

        // Create the pause menu.
        CreatePauseMenu(atlas, uiSoundEffect, controller);

        // Create the game over menu.
        CreateGameOverMenu(atlas, uiSoundEffect, controller);
    }

    private void CreatePauseMenu(TextureAtlas atlas, SoundEffect soundEffect, UIElementController controller)
    {
        // Create the root container for the paused menu.
        _pauseMenu = new UISprite();
        _pauseMenu.Sprite = atlas.CreateSprite("overlay-pixel");
        _pauseMenu.Sprite.Scale = Core.GraphicsDevice.PresentationParameters.Bounds.Size.ToVector2();
        _pauseMenu.Controller = controller;
        _pauseMenu.IsSelected = true;
        _pauseMenu.IsEnabled = _pauseMenu.IsVisible = false;

        // Create the paused panel as a child of the paused menu.
        UISprite pausePanel = _pauseMenu.CreateChild<UISprite>();
        pausePanel.Sprite = atlas.CreateSprite("panel");
        pausePanel.Position = new Vector2(215, 249);

        // Create the paused text as a child of the paused panel.
        UISprite pausedText = pausePanel.CreateChild<UISprite>();
        pausedText.Sprite = atlas.CreateSprite("paused-label");
        pausedText.Position = new Vector2(42, 42);

        // Create the resume button as a child of the paused panel.
        UIButton resumeButton = pausePanel.CreateChild<UIButton>();
        resumeButton.NotSelectedSprite = atlas.CreateSprite("resume-button");
        resumeButton.NotSelectedSprite.CenterOrigin();
        resumeButton.SelectedSprite = atlas.CreateAnimatedSprite("resume-button-selected");
        resumeButton.SelectedSprite.CenterOrigin();
        resumeButton.Position = new Vector2(148, 148);
        resumeButton.IsSelected = true;

        // Create the quite button as a child of the paused panel.
        UIButton quitButton = pausePanel.CreateChild<UIButton>();
        quitButton.NotSelectedSprite = atlas.CreateSprite("quit-button");
        quitButton.NotSelectedSprite.CenterOrigin();
        quitButton.SelectedSprite = atlas.CreateAnimatedSprite("quit-button-selected");
        quitButton.SelectedSprite.CenterOrigin();
        quitButton.Position = new Vector2(691, 148);

        // Wire up the actions to perform when the Right action is triggered
        // for the menu.
        _pauseMenu.LeftAction = () =>
        {
            // Play the sound effect
            Core.Audio.PlaySoundEffect(soundEffect);

            if (quitButton.IsSelected)
            {
                // The quit button is selected and the left action was
                // performed, so deselect the quit button and select the resume
                // button.
                quitButton.IsSelected = false;
                resumeButton.IsSelected = true;
            }
        };

        // Wire up the actions to perform when the Right action is triggered
        // for the menu.
        _pauseMenu.RightAction = () =>
        {
            // Play the sound effect
            Core.Audio.PlaySoundEffect(soundEffect);

            if (resumeButton.IsSelected)
            {
                // The resume button is selected and the right action was
                // performed, so deselect the resume button and select the quit
                // button
                resumeButton.IsSelected = false;
                quitButton.IsSelected = true;
            }
        };

        // Wire up the actions to perform when the Confirm action is triggered
        // for the menu.
        _pauseMenu.ConfirmAction = () =>
        {
            // Play the sound effect
            Core.Audio.PlaySoundEffect(soundEffect);

            if (resumeButton.IsSelected)
            {
                // The resume button is selected and the confirm action was
                // performed, so unpause the game by disabling the pause menu.
                _pauseMenu.IsEnabled = _pauseMenu.IsVisible = _pauseMenu.IsSelected = false;
            }
            else if (quitButton.IsSelected)
            {
                // The quit button is selected and the confirm action was
                // performed, so quit the game by changing the scene back to the
                // title scene.
                Core.ChangeScene(new TitleScene());
            }
        };

        // Wire up the actions to perform when the Cancel action is triggered
        // for the menu.
        _pauseMenu.CancelAction = () =>
        {
            // Play the sound effect.
            Core.Audio.PlaySoundEffect(soundEffect);

            // Unpause the game by disabling the paused menu.
            _pauseMenu.IsEnabled = _pauseMenu.IsVisible = _pauseMenu.IsSelected = false;
        };
    }

    private void CreateGameOverMenu(TextureAtlas atlas, SoundEffect soundEffect, UIElementController controller)
    {
        // Create the root container for the game over menu.
        _gameOverMenu = new UISprite();
        _gameOverMenu.Sprite = atlas.CreateSprite("overlay-pixel");
        _gameOverMenu.Sprite.Scale = Core.GraphicsDevice.PresentationParameters.Bounds.Size.ToVector2();
        _gameOverMenu.Controller = controller;
        _gameOverMenu.IsSelected = true;
        _gameOverMenu.IsEnabled = _gameOverMenu.IsVisible = false;

        // Create the game over panel as a child of the game over menu.
        UISprite gameOverPanel = _gameOverMenu.CreateChild<UISprite>();
        gameOverPanel.Sprite = atlas.CreateSprite("panel");
        gameOverPanel.Position = new Vector2(215, 249);

        // Create the game over text as a child of the game over panel.
        UISprite gameOverText = gameOverPanel.CreateChild<UISprite>();
        gameOverText.Sprite = atlas.CreateSprite("game-over-label");
        gameOverText.Position = new Vector2(42, 42);

        // Create the retry button as a child of the game over panel.
        UIButton retryButton = gameOverPanel.CreateChild<UIButton>();
        retryButton.NotSelectedSprite = atlas.CreateSprite("retry-button");
        retryButton.NotSelectedSprite.CenterOrigin();
        retryButton.SelectedSprite = atlas.CreateAnimatedSprite("retry-button-selected");
        retryButton.SelectedSprite.CenterOrigin();
        retryButton.Position = new Vector2(148, 148);

        // Create the quit button as a child of the game over panel.
        UIButton quitButton = gameOverPanel.CreateChild<UIButton>();
        quitButton.NotSelectedSprite = atlas.CreateSprite("quit-button");
        quitButton.NotSelectedSprite.CenterOrigin();
        quitButton.SelectedSprite = atlas.CreateAnimatedSprite("quit-button-selected");
        quitButton.SelectedSprite.CenterOrigin();
        quitButton.Position = new Vector2(691, 148);

        // Wire up the actions to perform when the Right action is triggered
        // for the menu.
        _gameOverMenu.LeftAction = () =>
        {
            // Play the sound effect
            Core.Audio.PlaySoundEffect(soundEffect);

            if (quitButton.IsSelected)
            {
                // The quit button is selected and the left action was
                // performed, so deselect the quit button and select the retry
                // button.
                quitButton.IsSelected = false;
                retryButton.IsSelected = true;
            }
        };

        // Wire up the actions to perform when the Right action is triggered
        // for the menu.
        _gameOverMenu.RightAction = () =>
        {
            // Play the sound effect
            Core.Audio.PlaySoundEffect(soundEffect);

            if (retryButton.IsSelected)
            {
                // The retry button is selected and the right action was
                // performed, so deselect the retry button and select the quit
                // button.
                retryButton.IsSelected = false;
                quitButton.IsSelected = true;
            }
        };

        // Wire up the actions to perform when the Confirm action is triggered
        // for the menu.
        _gameOverMenu.ConfirmAction = () =>
        {
            // Play the sound effect
            Core.Audio.PlaySoundEffect(soundEffect);

            if (retryButton.IsSelected)
            {
                // The retry button is selected and the confirm action was
                // performed, so deselect the game over menu.
                _gameOverMenu.IsEnabled = _gameOverMenu.IsVisible = _gameOverMenu.IsSelected = false;
            }
            else if (quitButton.IsSelected)
            {
                // The quite button is selected and the confirm action was
                // performed, so change the scene back to the title scene.
                Core.ChangeScene(new TitleScene());
            }
        };
    }

    public override void Update(GameTime gameTime)
    {
        _pauseMenu.Update(gameTime);
        _gameOverMenu.Update(gameTime);
        if (!_pauseMenu.IsEnabled && !_gameOverMenu.IsEnabled)
        {
            UpdateGame(gameTime);
        }
    }

    private void UpdateGame(GameTime gameTime)
    {
        // Update the slime animated sprite.
        _slime.Update(gameTime);

        // Update the bat animated sprite.
        _bat.Update(gameTime);

        // Check for keyboard input and handle it.
        CheckKeyboardInput();

        // Check for gamepad input and handle it.
        CheckGamePadInput();

        // Creating a bounding circle for the slime
        Circle slimeBounds = new Circle(
            (int)(_slimePosition.X + (_slime.Width * 0.5f)),
            (int)(_slimePosition.Y + (_slime.Height * 0.5f)),
            (int)(_slime.Width * 0.5f)
        );

        // Use distance based checks to determine if the slime is within the
        // bounds of the game screen, and if it's outside that screen edge,
        // move it back inside.
        if (slimeBounds.Left < _roomBounds.Left)
        {
            _slimePosition.X = _roomBounds.Left;
        }
        else if (slimeBounds.Right > _roomBounds.Right)
        {
            _slimePosition.X = _roomBounds.Right - _slime.Width;
        }

        if (slimeBounds.Top < _roomBounds.Top)
        {
            _slimePosition.Y = _roomBounds.Top;
        }
        else if (slimeBounds.Bottom > _roomBounds.Bottom)
        {
            _slimePosition.Y = _roomBounds.Bottom - _slime.Height;
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
        if (batBounds.Left < _roomBounds.Left)
        {
            normal.X = Vector2.UnitX.X;
            newBatPosition.X = _roomBounds.Left;
        }
        else if (batBounds.Right > _roomBounds.Right)
        {
            normal.X = -Vector2.UnitX.X;
            newBatPosition.X = _roomBounds.Right - _bat.Width;
        }

        if (batBounds.Top < _roomBounds.Top)
        {
            normal.Y = Vector2.UnitY.Y;
            newBatPosition.Y = _roomBounds.Top;
        }
        else if (batBounds.Bottom > _roomBounds.Bottom)
        {
            normal.Y = -Vector2.UnitY.Y;
            newBatPosition.Y = _roomBounds.Bottom - _bat.Height;
        }

        // If the normal is anything but Vector2.Zero, this means the bat had
        // moved outside the screen edge so we should reflect it about the
        // normal.
        if (normal != Vector2.Zero)
        {
            _batVelocity = Vector2.Reflect(_batVelocity, normal);

            // Play the bounce sound effect
            Core.Audio.PlaySoundEffect(_bounceSoundEffect);
        }

        _batPosition = newBatPosition;

        if (slimeBounds.Intersects(batBounds))
        {
            // Choose a random row and column based on the total number of each
            int column = Random.Shared.Next(1, _tilemap.Columns - 1);
            int row = Random.Shared.Next(1, _tilemap.Rows - 1);

            // Change the bat position by setting the x and y values equal to
            // the column and row multiplied by the width and height.
            _batPosition = new Vector2(column * _bat.Width, row * _bat.Height);

            // Assign a new random velocity to the bat
            AssignRandomBatVelocity();

            // Play the collect sound effect
            Core.Audio.PlaySoundEffect(_collectSoundEffect);

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
        // Get a reference to the keyboard info
        KeyboardInfo keyboard = Core.Input.Keyboard;

        // If the escape key is pressed, return to the title screen
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape))
        {
            _pauseMenu.IsEnabled = _pauseMenu.IsVisible = _pauseMenu.IsSelected = true;
        }

        // If the space key is held down, the movement speed increases by 1.5
        float speed = MOVEMENT_SPEED;
        if (keyboard.IsKeyDown(Keys.Space))
        {
            speed *= 1.5f;
        }

        // If the W or Up keys are down, move the slime up on the screen.
        if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up))
        {
            _slimePosition.Y -= speed;
        }

        // if the S or Down keys are down, move the slime down on the screen.
        if (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down))
        {
            _slimePosition.Y += speed;
        }

        // If the A or Left keys are down, move the slime left on the screen.
        if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))
        {
            _slimePosition.X -= speed;
        }

        // If the D or Right keys are down, move the slime right on the screen.
        if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))
        {
            _slimePosition.X += speed;
        }

        // If the M key is pressed, toggle mute state for audio.
        if (keyboard.WasKeyJustPressed(Keys.M))
        {
            Core.Audio.ToggleMute();
        }

        // If the + button is pressed, increase the volume.
        if (keyboard.WasKeyJustPressed(Keys.OemPlus))
        {
            Core.Audio.IncreaseVolume(0.1f);
        }

        // If the - button was pressed, decrease the volume.
        if (keyboard.WasKeyJustPressed(Keys.OemMinus))
        {
            Core.Audio.DecreaseVolume(0.1f);
        }
    }

    private void CheckGamePadInput()
    {
        // Get the gamepad info for gamepad one.
        GamePadInfo gamePadOne = Core.Input.GamePads[(int)PlayerIndex.One];

        if (gamePadOne.WasButtonJustPressed(Buttons.Start))
        {
            _pauseMenu.IsEnabled = _pauseMenu.IsVisible = _pauseMenu.IsSelected = true;
        }

        // If the A button is held down, the movement speed increases by 1.5
        // and the gamepad vibrates as feedback to the player.
        float speed = MOVEMENT_SPEED;
        if (gamePadOne.IsButtonDown(Buttons.A))
        {
            speed *= 1.5f;
            GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
        }
        else
        {
            GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
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

    public override void Draw(GameTime gameTime)
    {
        // Clear the back buffer.
        Core.GraphicsDevice.Clear(new Color(32, 40, 78, 255));

        // Begin the sprite batch to prepare for rendering.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Draw the tilemap
        _tilemap.Draw(Core.SpriteBatch);

        // Draw the slime sprite.
        _slime.Draw(Core.SpriteBatch, _slimePosition);

        // Draw the bat sprite.
        _bat.Draw(Core.SpriteBatch, _batPosition);

        // Draw the score
        Core.SpriteBatch.DrawString(
            _font,              // spriteFont
            $"Score: {_score}", // text
            _scoreTextPosition, // position
            Color.White,        // color
            0.0f,               // rotation
            _scoreTextOrigin,   // origin
            1.0f,               // scale
            SpriteEffects.None, // effects
            0.0f                // layerDepth
        );

        _pauseMenu.Draw(Core.SpriteBatch);
        _gameOverMenu.Draw(Core.SpriteBatch);

        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();
    }
}