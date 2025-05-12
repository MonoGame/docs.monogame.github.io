private void HandleQuitButtonClicked(object sender, EventArgs e)
{
    // A UI interaction occurred, play the sound effect
    Core.Audio.PlaySoundEffect(_uiSoundEffect);

    // Go back to the title scene.
    Core.ChangeScene(new TitleScene());
}