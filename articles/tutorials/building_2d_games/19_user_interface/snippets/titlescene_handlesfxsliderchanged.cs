private void HandleSfxSliderChanged(object sender, EventArgs args)
{
    var slider = (Slider)sender;
    Core.Audio.SoundEffectVolume = (float)slider.Value;
}