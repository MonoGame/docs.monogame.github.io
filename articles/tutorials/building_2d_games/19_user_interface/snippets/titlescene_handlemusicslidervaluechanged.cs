private static void HandleMusicSliderValueChanged(object sender, EventArgs args)
{
    var slider = (Slider)sender;
    Core.Audio.SongVolume = (float)slider.Value;
}