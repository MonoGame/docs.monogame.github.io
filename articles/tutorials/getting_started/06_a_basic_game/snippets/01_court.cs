using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BatAndBall;

public class Game1 : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _batTexture;
    private Texture2D _ballTexture;

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
    }

    protected override void Update(GameTime gameTime)
    {
#if !__IOS__
        // Exit the game if the Back button (GamePad) or Escape key (Keyboard) is pressed.
        // (iOS does not allow programmatically closing an app.)
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
#endif

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

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
