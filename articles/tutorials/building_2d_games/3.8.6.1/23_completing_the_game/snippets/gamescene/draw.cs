public override void Draw(GameTime gameTime)
{
    // Clear the back buffer.
    Core.GraphicsDevice.Clear(Color.CornflowerBlue);

    // Begin the sprite batch to prepare for rendering.
    Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

    // Draw the tilemap
    _tilemap.Draw(Core.SpriteBatch);

    // Draw the slime.
    _slime.Draw();

    // Draw the bat.
    _bat.Draw();

    // Always end the sprite batch when finished.
    Core.SpriteBatch.End();

    // Draw the UI.
    _ui.Draw();
}