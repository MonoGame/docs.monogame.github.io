private void HandleOptionsButtonBack(object sender, EventArgs e)
{
    // A UI interaction occurred, play the sound effect
    Core.Audio.PlaySoundEffect(_uiSoundEffect);

    // Set the title panel to be visible.
    _titleScreenButtonsPanel.IsVisible = true;

    // Set the options panel to be invisible.
    _optionsPanel.IsVisible = false;

    // Give the options button on the title panel focus since we are coming
    // back from the options screen.
    _optionsButton.IsFocused = true;
}