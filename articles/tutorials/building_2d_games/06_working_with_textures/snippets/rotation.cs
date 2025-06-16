protected override void Draw(GameTime gameTime)
{
    // Clear the back buffer.
    GraphicsDevice.Clear(Color.CornflowerBlue);

    // Begin the sprite batch to prepare for rendering.
    SpriteBatch.Begin();

    // Draw the texture.
    SpriteBatch.Draw(
        _logo,                      // texture
        new Vector2(                // position
            (Window.ClientBounds.Width * 0.5f) - (_logo.Width * 0.5f),
            (Window.ClientBounds.Height * 0.5f) - (_logo.Height * 0.5f)),
        null,                       // sourceRectangle
        Color.White,                // color
        MathHelper.ToRadians(90),   // rotation
        Vector2.Zero,               // origin
        1.0f,                       // scale
        SpriteEffects.None,         // effects
        0.0f                        // layerDepth
    );

    // Always end the sprite batch when finished.
    SpriteBatch.End();

    base.Draw(gameTime);
}