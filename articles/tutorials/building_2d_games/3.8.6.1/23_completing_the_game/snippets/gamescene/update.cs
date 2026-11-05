public override void Update(GameTime gameTime)
{
    // Ensure the UI is always updated.
    _ui.Update(gameTime);

    // If the game is in a game over state, immediately return back
    // here.
    if (_state == GameState.GameOver)
    {
        return;
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