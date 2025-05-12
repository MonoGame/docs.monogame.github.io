private void HandleMusicSliderValueChangeCompleted(object sender, EventArgs args)
{
    // A UI interaction occurred, play the sound effect
    Core.Audio.PlaySoundEffect(_uiSoundEffect);
}