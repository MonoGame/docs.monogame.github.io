private void HandleSfxSliderChangeCompleted(object sender, EventArgs e)
{
    // Play the UI Sound effect so the player can hear the difference in audio.
    Core.Audio.PlaySoundEffect(_uiSoundEffect);
}