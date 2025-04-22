private void HandleStartClicked(object sender, EventArgs e)
{
    // A UI interaction occurred, play the sound effect
    Core.Audio.PlaySoundEffect(_uiSoundEffect);

    // Change to the game scene to start the game.
    Core.ChangeScene(new GameScene());
}