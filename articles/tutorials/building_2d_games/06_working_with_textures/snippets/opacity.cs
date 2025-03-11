protected override void Draw(GameTime gameTime)
{
    // Clear the back buffer.
    GraphicsDevice.Clear(Color.CornflowerBlue);

    // Begin the sprite batch to prepare for rendering.
    _spriteBatch.Begin();

    // Draw the texture
    _spriteBatch.Draw(
        _logo,              // texture
        new Vector2(        // position
            Window.ClientBounds.Width,
            Window.ClientBounds.Height) * 0.5f,
        null,               // sourceRectangle
        Color.White * 0.5f, // color
        0.0f,               // rotation
        new Vector2(        // origin
            _logo.Width,
            _logo.Height) * 0.5f,
        1.0f,               // scale
        SpriteEffects.None, // effects
        0.0f
    );

    // Always end the sprite batch when finished.
    _spriteBatch.End();

    base.Draw(gameTime);
}