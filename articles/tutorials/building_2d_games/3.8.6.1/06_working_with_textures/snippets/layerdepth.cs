protected override void Draw(GameTime gameTime)
{
    // Clear the back buffer.
    GraphicsDevice.Clear(Color.CornflowerBlue);

    // The bounds of the icon within the texture.
    Rectangle iconSourceRect = new Rectangle(0, 0, 128, 128);

    // The bounds of the word mark within the texture.
    Rectangle wordmarkSourceRect = new Rectangle(150, 34, 458, 58);

    // Begin the sprite batch to prepare for rendering.
    SpriteBatch.Begin();

    // Draw only the icon portion of the texture.
    SpriteBatch.Draw(
        _logo,              // texture
        new Vector2(        // position
            Window.ClientBounds.Width,
            Window.ClientBounds.Height) * 0.5f,
        iconSourceRect,     // sourceRectangle
        Color.White,        // color
        0.0f,               // rotation
        new Vector2(        // origin
            iconSourceRect.Width,
            iconSourceRect.Height) * 0.5f,
        1.0f,               // scale
        SpriteEffects.None, // effects
        1.0f                // layerDepth
    );

    // Draw only the word mark portion of the texture.
    SpriteBatch.Draw(
        _logo,              // texture
        new Vector2(        // position
          Window.ClientBounds.Width,
          Window.ClientBounds.Height) * 0.5f,
        wordmarkSourceRect, // sourceRectangle
        Color.White,        // color
        0.0f,               // rotation
        new Vector2(        // origin
          wordmarkSourceRect.Width,
          wordmarkSourceRect.Height) * 0.5f,
        1.0f,               // scale
        SpriteEffects.None, // effects
        0.0f                // layerDepth
    );

    // Always end the sprite batch when finished.
    SpriteBatch.End();

    base.Draw(gameTime);
}