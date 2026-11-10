private void PauseGame()
{
    // Make the pause panel UI element visible.
    _pausePanel.IsVisible = true;

    // Set the resume button to have focus
    _resumeButton.IsFocused = true;
}
