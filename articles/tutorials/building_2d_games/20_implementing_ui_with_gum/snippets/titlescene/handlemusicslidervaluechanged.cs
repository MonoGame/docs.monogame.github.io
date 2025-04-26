private void HandleMusicSliderValueChanged(object sender, EventArgs args)
{
    // Intentionally not playing the UI sound effect here so that it's not
    // constantly triggered as the user adjusts the slider's thumb on the
    // track.

    // Get a reference to the sender as a Slider.
    var slider = (Slider)sender;

    // Set the global song volume to the value of the slider.
    Core.Audio.SongVolume = (float)slider.Value;
}