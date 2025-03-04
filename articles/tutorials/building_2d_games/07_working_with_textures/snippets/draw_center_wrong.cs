protected override void Draw(GameTime gameTime)
{
    // Clear the back buffer.
    GraphicsDevice.Clear(Color.CornflowerBlue);

    // Begin the sprite batch to prepare for rendering.
    _spriteBatch.Begin();

    // Draw the texture
    _spriteBatch.Draw(
        _logo,          // texture
        new Vector2(    // position
        Window.ClientBounds.Width, Window.ClientBounds.Height) * 0.5f,
        Color.White     // color
    );

    // Always end the sprite batch when finished.
    _spriteBatch.End();

    base.Draw(gameTime);
}
