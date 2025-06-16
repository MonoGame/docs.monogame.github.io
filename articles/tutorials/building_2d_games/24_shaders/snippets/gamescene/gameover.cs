private void GameOver()
{
    // Show the game over panel.
    _ui.ShowGameOverPanel();

    // Set the game state to game over.
    _state = GameState.GameOver;

    // Set the grayscale effect saturation to 1.0f
    _saturation = 1.0f;
}