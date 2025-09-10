public override void Update(GameTime gameTime)
{
    // Ensure the UI is always updated
    _ui.Update(gameTime);

    // Update the colorSwap material if it was changed
    _colorSwapMaterial.Update();

    if (_state != GameState.Playing)
    {
        // The game is in either a paused or game over state, so
        // gradually decrease the saturation to create the fading grayscale.
        _saturation = Math.Max(0.0f, _saturation - FADE_SPEED);

        // If its just a game over state, return back
        if (_state == GameState.GameOver)
        {
            return;
        }
    }
    else
    {
        _saturation = 1;
    }

    // ...
}