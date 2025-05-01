/// <summary>
/// Updates the text on the score display.
/// </summary>
/// <param name="score">The score to display.</param>
public void UpdateScoreText(int score)
{
    _scoreText.Text = string.Format(s_scoreFormat, score);
}

/// <summary>
/// Tells the game scene ui to show the pause panel.
/// </summary>
public void ShowPausePanel()
{
    // Make the overlay and the pause panel visible.
    _overlay.Visible = true;
    _pausePanel.IsVisible = true;

    // Give the resume button focus for keyboard/gamepad input.
    _resumeButton.IsFocused = true;

    // Ensure the game over panel isn't visible.
    _gameOverPanel.IsVisible = false;
}

/// <summary>
/// Tells the game scene ui to hide the pause panel.
/// </summary>
public void HidePausePanel()
{
    // Hide the overlay and the pause panel.
    _overlay.Visible = false;
    _pausePanel.IsVisible = false;
}

/// <summary>
/// Tells the game scene ui to show the game over panel.
/// </summary>
public void ShowGameOverPanel()
{
    // Make the overlay and the game over panel visible.
    _overlay.Visible = true;
    _gameOverPanel.IsVisible = true;

    // Give the retry button focus for keyboard/gamepad input.
    _retryButton.IsFocused =true;

    // Ensure the pause panel isn't visible.
    _pausePanel.IsVisible = false;
}

/// <summary>
/// Tells the game scene ui to hide the game over panel.
/// </summary>
public void HideGameOverPanel()
{
    // Hide the overlay and game over panel.
    _overlay.Visible = false;
    _gameOverPanel.IsVisible = false;
}

/// <summary>
/// Updates the game scene ui.
/// </summary>
/// <param name="gameTime">A snapshot of the timing values for the current update cycle.</param>
public void Update(GameTime gameTime)
{
    GumService.Default.Update(gameTime);
}

/// <summary>
/// Draws the game scene ui.
/// </summary>
public void Draw()
{
    GumService.Default.Draw();
}