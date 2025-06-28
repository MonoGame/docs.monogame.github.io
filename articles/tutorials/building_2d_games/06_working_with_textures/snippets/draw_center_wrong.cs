protected override void Draw(GameTime gameTime)
{
    // Clear the back buffer.
    GraphicsDevice.Clear(Color.CornflowerBlue);

    // Begin the sprite batch to prepare for rendering.
    SpriteBatch.Begin();

    // Draw the logo texture.
    SpriteBatch.Draw(
        _logo,          // texture
        new Vector2(    // position
            Window.ClientBounds.Width, 
            Window.ClientBounds.Height)
            * 0.5f,
        Color.White     // color
    );

    // Always end the sprite batch when finished.
    SpriteBatch.End();

    base.Draw(gameTime);
}
