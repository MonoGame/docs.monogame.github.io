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
    private const string PRESS_ENTER = "Press Enter To Start";

    // The font to use to render normal text.
    private SpriteFont _font;

    // The sprite to draw for the stylized title
    private Sprite _titleSprite;

    // The position to draw the title sprite at.
    private Vector2 _titlePos;

    // The position to draw the press enter text at.
    private Vector2 _pressEnterPos;

    // The origin to set for the press enter text when drawing it.
    private Vector2 _pressEnterOrigin;
    #endregion

    #region initialize
    public override void Initialize()
    {
        // LoadContent is called during base.Initialize().
        base.Initialize();

        // While on the title screen, we can enable exit on escape so the player
        // can close the game by pressing the escape key.
        Core.ExitOnEscape = true;

        // Get the bounds of the screen for position calculations
        Rectangle screenBounds = Core.GraphicsDevice.PresentationParameters.Bounds;

        // Precalculate the positions and origins for texts and the slime sprite
        // so we're not calculating it every draw frame.
        _titlePos = new Vector2(
            screenBounds.Width * 0.5f,
            80 + _titleSprite.Height * 0.5f);

        // Center the origin of the title sprite.
        _titleSprite.CenterOrigin();

        // Precalculate the position of for the press enter text so that it is
        // centered horizontally and place 100 pixels above the bottom of the
        // screen.
        _pressEnterPos = new Vector2(
            screenBounds.Width * 0.5f,
            screenBounds.Height - 100
        );

        // Precalculate the center origin of the press enter text.
        Vector2 pressEnterSize = _font.MeasureString(PRESS_ENTER);
        _pressEnterOrigin = pressEnterSize * 0.5f;
    }
    #endregion

    #region loadcontent
    public override void LoadContent()
    {
        // Load the font for the standard txt.
        _font = Core.Content.Load<SpriteFont>("fonts/gameFont");

        // Create a texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/atlas-definition.xml");

        _titleSprite = atlas.CreateSprite("title-card");
    }
    #endregion

    #region update
    public override void Update(GameTime gameTime)
    {
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

        // Draw that title sprite
        _titleSprite.Draw(Core.SpriteBatch, _titlePos);

        // Draw the press enter text
        Core.SpriteBatch.DrawString(_font, PRESS_ENTER, _pressEnterPos, Color.White, 0.0f, _pressEnterOrigin, 1.0f, SpriteEffects.None, 0.0f);

        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();
    }
    #endregion
}