private void HandleResumeButtonClicked(object sender, EventArgs e)
{
    // A UI interaction occurred, play the sound effect
    Core.Audio.PlaySoundEffect(_uiSoundEffect);

    // Make the pause panel invisible to resume the game.
    _pausePanel.IsVisible = false;
}