#region declaration
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Scenes;

namespace DungeonSlime.Scenes;

public class TitleScene : Scene
{

}
#endregion
{
    #region fields
    private const string TITLE = "Dungeon Slime";
    private const string PRESS_ENTER = "Press Enter To Start";

    // The font to use to render the title text
    private SpriteFont _titleFont;

    // The font to use to render normal text.
    private SpriteFont _standardFont;

    // The position to draw the title text at.
    private Vector2 _titlePos;

    // The origin to set for the title text when drawing it.
    private Vector2 _titleOrigin;

    // The position to draw the press enter text at.
    private Vector2 _pressEnterPos;

    // The origin to set for the press enter text when drawing it.
    private Vector2 _pressEnterOrigin;

    // The slime animation to give the title screen some life.
    private AnimatedSprite _slime;

    //  The position to draw the slime animation at.
    private Vector2 _slimePos;
    #endregion

    #region initialize
    public override void Initialize()
    {
        // LoadContent is called during base.Initialize().
        base.Initialize();

        // While on the title screen, we can enable exit on escape so the player
        // can close the game by pressing the escape key.
        Core.ExitOnEscape = true;

        // Precalculate the positions and origins for texts and the slime sprite
        // so we're not calculating it every draw frame.
        _titlePos = new Vector2(
            Core.GraphicsDevice.PresentationParameters.BackBufferWidth * 0.5f,
            100);

        Vector2 titleSize = _titleFont.MeasureString(TITLE);
        _titleOrigin = titleSize * 0.5f;

        _pressEnterPos = new Vector2(
            Core.GraphicsDevice.PresentationParameters.BackBufferWidth * 0.5f,
            Core.GraphicsDevice.PresentationParameters.BackBufferHeight - 100
        );

        Vector2 pressEnterSize = _standardFont.MeasureString(PRESS_ENTER);
        _pressEnterOrigin = pressEnterSize * 0.5f;

        _slimePos = new Vector2(
            Core.GraphicsDevice.PresentationParameters.BackBufferWidth,
            Core.GraphicsDevice.PresentationParameters.BackBufferHeight
        ) * 0.5f;

        _slime.CenterOrigin();
        _slime.Scale = new Vector2(5.0f, 5.0f);
    }
    #endregion

    #region loadcontent
    public override void LoadContent()
    {
        // Load the font for the title text
        _titleFont = Content.Load<SpriteFont>("fonts/titleFont");

        // Load the font for the standard txt.
        _standardFont = Core.Content.Load<SpriteFont>("fonts/gameFont");

        // Create a texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/atlas-definition.xml");

        // Create the slime animated sprite from the atlas.
        _slime = atlas.CreateAnimatedSprite("slime-animation");
    }
    #endregion

    #region update
    public override void Update(GameTime gameTime)
    {
        // Update the sprite
        _slime.Update(gameTime);

        // If the user presses enter, switch to the game scene.
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter))
        {
            Core.ChangeScene(new GameScene());
        }
    }
    #endregion

    #region draw
    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(new Color(32, 40, 78, 255));

        // Begin the sprite batch to prepare for rendering.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Draw that title text
        Core.SpriteBatch.DrawString(_titleFont, TITLE, _titlePos, Color.White, 0.0f, _titleOrigin, 1.0f, SpriteEffects.None, 0.0f);

        // Draw the press enter text
        Core.SpriteBatch.DrawString(_standardFont, PRESS_ENTER, _pressEnterPos, Color.White, 0.0f, _pressEnterOrigin, 1.0f, SpriteEffects.None, 0.0f);

        // Draw the animated slime
        _slime.Draw(Core.SpriteBatch, _slimePos);

        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();
    }
    #endregion
}