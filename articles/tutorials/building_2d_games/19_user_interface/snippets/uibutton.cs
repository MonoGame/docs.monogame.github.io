using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.UI;

/// <summary>
/// A UI element that represents a button with different visual states
/// for selected and non-selected states.
/// </summary>
public class UIButton : UIElement
{
    /// <summary>
    /// Gets or sets the sprite used when the button is not selected.
    /// </summary>
    public Sprite NotSelectedSprite { get; set; }

    /// <summary>
    /// Gets or sets the sprite used when the button is selected.
    /// </summary>
    public Sprite SelectedSprite { get; set; }

    /// <summary>
    /// Updates the button's state and animations.
    /// </summary>
    /// <param name="gameTime">Time elapsed since the last update.</param>
    public override void Update(GameTime gameTime)
    {
        // Update animations if sprites are animated
        if (NotSelectedSprite is AnimatedSprite notSelectedSprite)
        {
            notSelectedSprite.Update(gameTime);
        }

        if (SelectedSprite is AnimatedSprite selectedSprite)
        {
            selectedSprite.Update(gameTime);
        }

        base.Update(gameTime);
    }

    /// <summary>
    /// Draws the appropriate button sprite based on selection state.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch to use for drawing.</param>
    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!IsVisible)
        {
            return;
        }

        // Draw different sprites based on selection state
        if (IsSelected)
        {
            SelectedSprite.Color = IsEnabled ? EnabledColor : DisabledColor;
            SelectedSprite.Draw(spriteBatch, AbsolutePosition);
        }
        else
        {
            NotSelectedSprite.Color = IsEnabled ? EnabledColor : DisabledColor;
            NotSelectedSprite.Draw(spriteBatch, AbsolutePosition);
        }

        base.Draw(spriteBatch);
    }
}
