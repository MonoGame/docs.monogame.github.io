using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace DungeonSlime;

public class Game1 : Core
{
    // texture region that defines the slime sprite in the atlas.
    private TextureRegion _slime;

    // texture region that defines the bat sprite in the atlas.
    private TextureRegion _bat;

    public Game1() : base("Dungeon Slime", 1280, 720, false)
    {

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Load the atlas texture using the content manager
        Texture2D atlasTexture = Content.Load<Texture2D>("images/atlas");

        //  Create a TextureAtlas instance from the atlas
        TextureAtlas atlas = new TextureAtlas(atlasTexture);

        // add the slime region to the atlas.
        atlas.AddRegion("slime", 0, 0, 80, 80);

        // add the bat region to the atlas.
        atlas.AddRegion("bat", 0, 80, 80, 80);

        // retrieve the slime region from the atlas.
        _slime = atlas.GetRegion("slime");

        // retrieve the bat region from the atlas.
        _bat = atlas.GetRegion("bat");

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Clear the back buffer.
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Begin the sprite batch to prepare for rendering.
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Draw the slime texture region.
        _slime.Draw(SpriteBatch, Vector2.One, Color.White);

        // Draw the bat texture region 10px to the right of the slime.
        _bat.Draw(SpriteBatch, new Vector2(_slime.Width + 10, 0), Color.White);

        // Always end the sprite batch when finished.
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}