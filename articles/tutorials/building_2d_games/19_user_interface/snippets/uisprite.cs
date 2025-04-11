using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.UI;

/// <summary>
/// A simple UI element that displays a sprite.
/// Useful for static elements like icons, backgrounds, and decorations.
/// </summary>
public class UISprite : UIElement
{
    /// <summary>
    /// Gets or sets the sprite to display.
    /// </summary>
    public Sprite Sprite { get; set; }

    /// <summary>
    /// Updates the sprite, handling animation if the sprite is animated.
    /// </summary>
    /// <param name="gameTime">Time elapsed since the last update.</param>
    public override void Update(GameTime gameTime)
    {
        if (!IsEnabled)
        {
            return;
        }

        // Update animation if the sprite is an animated sprite
        if (Sprite is AnimatedSprite animatedSprite)
        {
            animatedSprite.Update(gameTime);
        }

        base.Update(gameTime);
    }

    /// <summary>
    /// Draws the sprite with the appropriate color based on enabled state.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch to use for drawing.</param>
    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!IsVisible)
        {
            return;
        }

        if (Sprite != null)
        {
            // Apply enabled/disabled color to the sprite
            Sprite.Color = IsEnabled ? EnabledColor : DisabledColor;
            Sprite.Draw(spriteBatch, AbsolutePosition);
        }

        base.Draw(spriteBatch);
    }
}
