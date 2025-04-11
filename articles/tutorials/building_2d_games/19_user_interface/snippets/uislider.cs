using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.UI;

/// <summary>
/// A UI element that represents a slider control with adjustable value
/// that can be increased or decreased in steps.
/// </summary>
public class UISlider : UIElement
{
    /// <summary>
    /// Gets or sets the background sprite for the slider.
    /// </summary>
    public Sprite SliderSprite { get; set; }

    /// <summary>
    /// Gets or sets the sprite used for the fill bar that represents the current value.
    /// </summary>
    public Sprite FillSprite { get; set; }

    /// <summary>
    /// Gets or sets the rectangle that defines the position and size of the fill area
    /// relative to the slider's position.
    /// </summary>
    public Rectangle FillBounds { get; set; }

    /// <summary>
    /// Gets or sets the minimum value of the slider.
    /// </summary>
    public float MinValue { get; set; }

    /// <summary>
    /// Gets or sets the maximum value of the slider.
    /// </summary>
    public float MaxValue { get; set; }

    /// <summary>
    /// Gets or sets the current value of the slider.
    /// </summary>
    public float Value { get; set; }

    /// <summary>
    /// Gets or sets the amount to change the value by when stepping up or down.
    /// </summary>
    public float Step { get; set; }

    /// <summary>
    /// Increases the slider value by one step, clamped to the maximum.
    /// </summary>
    /// <returns>The new value after stepping up.</returns>
    public float StepUp()
    {
        Value = Math.Clamp(Value + Step, MinValue, MaxValue);
        return Value;
    }

    /// <summary>
    /// Decreases the slider value by one step, clamped to the minimum.
    /// </summary>
    /// <returns>The new value after stepping down.</returns>
    public float StepDown()
    {
        Value = Math.Clamp(Value - Step, MinValue, MaxValue);
        return Value;
    }

    /// <summary>
    /// Draws the slider with the fill representing the current value.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch to use for drawing.</param>
    public override void Draw(SpriteBatch spriteBatch)
    {
        // Calculate the position of the fill sprite relative to the slider
        Vector2 fillSpritePosition = AbsolutePosition + FillBounds.Location.ToVector2();

        // Calculate the fill percentage based on the current value
        float percentage = (Value - MinValue) / (MaxValue - MinValue);
        // Scale the fill sprite width according to the current value percentage
        FillSprite.Scale = new Vector2(FillBounds.Width * percentage, FillBounds.Height);

        // Draw the fill bar with appropriate color
        FillSprite.Color = IsEnabled ? EnabledColor : DisabledColor;
        FillSprite.Draw(spriteBatch, fillSpritePosition);

        // Draw the slider background with appropriate color
        SliderSprite.Color = IsEnabled ? EnabledColor : DisabledColor;
        SliderSprite.Draw(spriteBatch, AbsolutePosition);

        base.Draw(spriteBatch);
    }
}
