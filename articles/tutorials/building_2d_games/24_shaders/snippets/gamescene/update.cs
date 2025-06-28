public override void Update(GameTime gameTime)
{
    // Ensure the UI is always updated.
    _ui.Update(gameTime);

    if (_state != GameState.Playing)
    {
        // The game is in either a paused or game over state, so
        // gradually decrease the saturation to create the fading grayscale.
        _saturation = Math.Max(0.0f, _saturation - FADE_SPEED);

        // If its just a game over state, return back.
        if (_state == GameState.GameOver)
        {
            return;
        }
    }

    // If the pause button is pressed, toggle the pause state.
    if (GameController.Pause())
    {
        TogglePause();
    }

    // At this point, if the game is paused, just return back early.
    if (_state == GameState.Paused)
    {
        return;
    }

    // Update the slime.
    _slime.Update(gameTime);

    // Update the bat.
    _bat.Update(gameTime);

    // Perform collision checks.
    CollisionChecks();
}