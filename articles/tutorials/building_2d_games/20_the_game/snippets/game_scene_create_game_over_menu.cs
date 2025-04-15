private void CreateGameOverMenu(TextureAtlas atlas, SoundEffect soundEffect, UIElementController controller)
{
    // Prior existing code for this method...

    // Wire up the actions to perform when the Confirm action is triggered
    // for the menu.
    _gameOverMenu.ConfirmAction = () =>
    {
        // Play the sound effect
        Core.Audio.PlaySoundEffect(soundEffect);

        if (retryButton.IsSelected)
        {
            // The retry button is selected and the confirm action was
            // performed, so initialize a new game and hide the game over menu.
            InitializeNewGame();
            _gameOverMenu.IsEnabled = _gameOverMenu.IsVisible = _gameOverMenu.IsSelected = false;
        }
        else if (quitButton.IsSelected)
        {
            // The quite button is selected and the confirm action was
            // performed, so change the scene back to the title scene.
            Core.ChangeScene(new TitleScene());
        }
    };
}
