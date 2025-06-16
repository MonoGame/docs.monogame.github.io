public override void Draw(GameTime gameTime)
{
    // Clear the back buffer.
    Core.GraphicsDevice.Clear(Color.CornflowerBlue);

    if (_state != GameState.Playing)
    {
        // We are in a game over state, so apply the saturation parameter.
        _grayscaleEffect.Parameters["Saturation"].SetValue(_saturation);

        // And begin the sprite batch using the grayscale effect.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, effect: _grayscaleEffect);
    }
    else
    {
        // Otherwise, just begin the sprite batch as normal.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
    }

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