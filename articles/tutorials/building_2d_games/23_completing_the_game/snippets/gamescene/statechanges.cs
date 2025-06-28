private void OnSlimeBodyCollision(object sender, EventArgs args)
{
    GameOver();
}

private void TogglePause()
{
    if (_state == GameState.Paused)
    {
        // We're now unpausing the game, so hide the pause panel.
        _ui.HidePausePanel();

        // And set the state back to playing.
        _state = GameState.Playing;
    }
    else
    {
        // We're now pausing the game, so show the pause panel.
        _ui.ShowPausePanel();

        // And set the state to paused.
        _state = GameState.Paused;
    }
}

private void GameOver()
{
    // Show the game over panel.
    _ui.ShowGameOverPanel();

    // Set the game state to game over.
    _state = GameState.GameOver;
}