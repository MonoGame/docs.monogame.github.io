private void HandleOptionsButtonBack(object sender, EventArgs e)
{
    _titleScreenButtonsPanel.IsVisible = true;
    _optionsPanel.IsVisible = false;

    _optionsButton.IsFocused = true;
}