using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Graphics;

namespace MonoGameSnake;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private TextureRegion _slime;
    private TextureRegion _bat;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load the atlas texture using the content manager
        Texture2D atlasTexture = Content.Load<Texture2D>("images/atlas");

        //  Create a TextureAtlas instance from the atlas
        TextureAtlas atlas = new TextureAtlas(atlasTexture);

        //  Create and add the slime and bad regions
        atlas.AddRegion("slime", 0, 160, 40, 40);
        atlas.AddRegion("bat", 80, 160, 40, 40);

        // Retrieve the slime and bat regions
        _slime = atlas.GetRegion("slime");
        _bat = atlas.GetRegion("bat");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        // Draw the slime texture region.
        _slime.Draw(_spriteBatch, Vector2.One, Color.White);

        // Draw the bat texture region 10px to the right of the slime.
        _bat.Draw(_spriteBatch, new Vector2(_slime.Width + 10, 0), Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
