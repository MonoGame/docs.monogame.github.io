protected override void Draw(GameTime gameTime)
{
    // Clear the back buffer.
    GraphicsDevice.Clear(Color.CornflowerBlue);

    // Begin the sprite batch to prepare for rendering.
    _spriteBatch.Begin();

    // Draw the texture
    _spriteBatch.Draw(
        _logo,                      // texture
        new Vector2(                // position
            Window.ClientBounds.Width,
            indow.ClientBounds.Height) * 0.5f,
        null,                       // sourceRectangle
        Color.White,                // color
        0.0f,                       // rotation
        new Vector2(                // origin
            _logo.Width,
            _logo.Height) * 0.5f,
        new Vector2(1.5f, 0.5f),    // scale
        SpriteEffects.None,         // effects
        0.0f                        // layerDepth
    );

    // Always end the sprite batch when finished.
    _spriteBatch.End();

    base.Draw(gameTime);
}