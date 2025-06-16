protected override void Draw(GameTime gameTime)
{
    // Clear the back buffer.
    GraphicsDevice.Clear(Color.CornflowerBlue);

    // Begin the sprite batch to prepare for rendering.
    SpriteBatch.Begin();

    // Draw the texture.
    SpriteBatch.Draw(_logo, Vector2.Zero, Color.White);

    // Always end the sprite batch when finished.
    SpriteBatch.End();

    base.Draw(gameTime);
}
