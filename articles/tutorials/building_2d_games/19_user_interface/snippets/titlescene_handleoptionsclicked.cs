private void HandleOptionsClicked(object sender, EventArgs e)
{
    _titleScreenButtonsPanel.IsVisible = false;

    _optionsPanel.IsVisible = true;
    _optionsBackButton.IsFocused = true;
}