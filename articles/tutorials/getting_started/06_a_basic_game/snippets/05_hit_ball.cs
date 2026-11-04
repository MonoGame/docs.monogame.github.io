using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BatAndBall;

public class Game1 : Microsoft.Xna.Framework.Game
{
    private const float BatSpeed = 400f;  // Pixels each second.
    private const float BallSpeed = 300f; // Pixels each second.

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _batTexture;
    private Texture2D _ballTexture;

    private Vector2 _leftBat;
    private Vector2 _rightBat;
    private Vector2 _ballPosition;
    private Vector2 _ballVelocity;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _batTexture = Content.Load<Texture2D>("bat");
        _ballTexture = Content.Load<Texture2D>("ball");

        // Put the bats at the left and right edges, in the middle of the screen.
        Rectangle screen = GraphicsDevice.Viewport.Bounds;
        float batY = (screen.Height - _batTexture.Height) / 2f;
        _leftBat = new Vector2(20, batY);
        _rightBat = new Vector2(screen.Width - 20 - _batTexture.Width, batY);

        // Serve the ball toward player 2.
        ResetBall(1);
    }

    protected override void Update(GameTime gameTime)
    {
#if !__IOS__
        // Exit the game if the Back button (GamePad) or Escape key (Keyboard) is pressed.
        // (iOS does not allow programmatically closing an app.)
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
#endif

        KeyboardState keyboard = Keyboard.GetState();
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Rectangle screen = GraphicsDevice.Viewport.Bounds;

        // Move the bats. Player 1 uses W and S. Player 2 uses Up and Down.
        _leftBat.Y += GetDirection(keyboard, Keys.W, Keys.S) * BatSpeed * elapsed;
        _rightBat.Y += GetDirection(keyboard, Keys.Up, Keys.Down) * BatSpeed * elapsed;
        _leftBat.Y = MathHelper.Clamp(_leftBat.Y, 0, screen.Height - _batTexture.Height);
        _rightBat.Y = MathHelper.Clamp(_rightBat.Y, 0, screen.Height - _batTexture.Height);

        // Move the ball.
        _ballPosition += _ballVelocity * elapsed;

        // Bounce the ball off the top and bottom edges.
        if ((_ballPosition.Y < 0 && _ballVelocity.Y < 0) ||
            (_ballPosition.Y > screen.Height - _ballTexture.Height && _ballVelocity.Y > 0))
        {
            _ballVelocity.Y = -_ballVelocity.Y;
        }

        // Bounce the ball off a bat.
        if (_ballVelocity.X < 0 && GetBounds(_ballPosition, _ballTexture).Intersects(GetBounds(_leftBat, _batTexture)))
        {
            HitBall(_leftBat);
        }
        else if (_ballVelocity.X > 0 && GetBounds(_ballPosition, _ballTexture).Intersects(GetBounds(_rightBat, _batTexture)))
        {
            HitBall(_rightBat);
        }

        // Serve again when the ball goes off the left or right edge.
        if (_ballPosition.X < -_ballTexture.Width)
        {
            ResetBall(-1);
        }
        else if (_ballPosition.X > screen.Width)
        {
            ResetBall(1);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        // Draw a gray dotted line down the middle, to show the two sides.
        int middle = GraphicsDevice.Viewport.Width / 2;
        for (int y = 5; y < GraphicsDevice.Viewport.Height; y += 20)
        {
            _spriteBatch.Draw(_ballTexture, new Rectangle(middle - 4, y, 8, 8), Color.White * 0.2f);
        }

        _spriteBatch.Draw(_batTexture, _leftBat, Color.White);
        _spriteBatch.Draw(_batTexture, _rightBat, Color.White);
        _spriteBatch.Draw(_ballTexture, _ballPosition, Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    // Returns -1 to move up, 1 to move down, or 0 to stay.
    private static float GetDirection(KeyboardState keyboard, Keys up, Keys down)
    {
        if (keyboard.IsKeyDown(up))
        {
            return -1;
        }

        if (keyboard.IsKeyDown(down))
        {
            return 1;
        }

        return 0;
    }

    // Returns the rectangle that a texture covers at a position.
    private static Rectangle GetBounds(Vector2 position, Texture2D texture)
    {
        return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
    }

    private void HitBall(Vector2 bat)
    {
        // Send the ball back 5% faster.
        _ballVelocity.X = -_ballVelocity.X * 1.05f;

        // Change the angle. A hit near the end of the bat gives a steeper angle.
        float ballCenter = _ballPosition.Y + _ballTexture.Height / 2f;
        float batCenter = bat.Y + _batTexture.Height / 2f;
        _ballVelocity.Y = (ballCenter - batCenter) * 6f;
    }

    private void ResetBall(int direction)
    {
        Rectangle screen = GraphicsDevice.Viewport.Bounds;
        _ballPosition = new Vector2(screen.Width - _ballTexture.Width, screen.Height - _ballTexture.Height) / 2f;

        // Start with a random vertical speed, so that each serve is different.
        float verticalSpeed = (Random.Shared.NextSingle() - 0.5f) * BallSpeed;
        _ballVelocity = new Vector2(direction * BallSpeed, verticalSpeed);
    }
}
