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

        // Set the grayscale effect saturation to 1.0f
        _saturation = 1.0f;
    }
}