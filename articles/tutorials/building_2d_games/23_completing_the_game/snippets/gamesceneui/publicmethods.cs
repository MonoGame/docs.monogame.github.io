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
    _pausePanel.IsVisible = false;
}

/// <summary>
/// Tells the game scene ui to show the game over panel.
/// </summary>
public void ShowGameOverPanel()
{
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
