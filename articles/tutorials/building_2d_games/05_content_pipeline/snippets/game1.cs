using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace DungeonSlime;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // The MonoGame logo texture
    private Texture2D _logo;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Create a new FramesPerSecondCounter.
        FramesPerSecondCounter fpsCounter = new FramesPerSecondCounter(this);

        // Add it to the game's component collection
        Components.Add(fpsCounter);
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load the MonoGame logo asset using the ContentManager
        _logo = Content.Load<Texture2D>("images/logo");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Clear the back buffer.
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Begin the sprite batch to prepare for rendering.
        _spriteBatch.Begin();

        // Draw the logo texture
        _spriteBatch.Draw(_logo, Vector2.Zero, Color.White);

        // Always end the sprite batch when finished.
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}