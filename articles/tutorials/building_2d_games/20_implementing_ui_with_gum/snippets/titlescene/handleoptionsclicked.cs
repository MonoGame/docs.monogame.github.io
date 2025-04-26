private void HandleOptionsClicked(object sender, EventArgs e)
{
    // A UI interaction occurred, play the sound effect
    Core.Audio.PlaySoundEffect(_uiSoundEffect);
    
    // Set the title panel to be invisible.
    _titleScreenButtonsPanel.IsVisible = false;

    // Set the options panel to be visible.
    _optionsPanel.IsVisible = true;

    // Give the back button on the options panel focus.
    _optionsBackButton.IsFocused = true;
}