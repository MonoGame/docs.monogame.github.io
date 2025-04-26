private void CreateOptionsPanel()
{
    _optionsPanel = new Panel();
    _optionsPanel.Dock(Gum.Wireframe.Dock.Fill);
    _optionsPanel.IsVisible = false;
    _optionsPanel.AddToRoot();

    TextRuntime optionsText = new TextRuntime();
    optionsText.X = 10;
    optionsText.Y = 10;
    optionsText.Text = "OPTIONS";
    optionsText.UseCustomFont = true;
    optionsText.FontScale = 0.5f;
    optionsText.CustomFontFile = @"fonts/04b_30.fnt";
    _optionsPanel.AddChild(optionsText);

    OptionsSlider musicSlider = new OptionsSlider(_atlas);
    musicSlider.Name = "MusicSlider";
    musicSlider.Text = "MUSIC";
    musicSlider.Anchor(Gum.Wireframe.Anchor.Top);
    musicSlider.Visual.Y = 30f;
    musicSlider.Minimum = 0;
    musicSlider.Maximum = 1;
    musicSlider.Value = Core.Audio.SongVolume;
    musicSlider.SmallChange = .1;
    musicSlider.LargeChange = .2;
    musicSlider.ValueChanged += HandleMusicSliderValueChanged;
    musicSlider.ValueChangeCompleted += HandleMusicSliderValueChangeCompleted;
    _optionsPanel.AddChild(musicSlider);

    OptionsSlider sfxSlider = new OptionsSlider(_atlas);
    sfxSlider.Name = "SfxSlider";
    sfxSlider.Text = "SFX";
    sfxSlider.Anchor(Gum.Wireframe.Anchor.Top);
    sfxSlider.Visual.Y = 93;
    sfxSlider.Minimum = 0;
    sfxSlider.Maximum = 1;
    sfxSlider.Value = Core.Audio.SoundEffectVolume;
    sfxSlider.SmallChange = .1;
    sfxSlider.LargeChange = .2;
    sfxSlider.ValueChanged += HandleSfxSliderChanged;
    sfxSlider.ValueChangeCompleted += HandleSfxSliderChangeCompleted;
    _optionsPanel.AddChild(sfxSlider);

    _optionsBackButton = new AnimatedButton(_atlas);
    _optionsBackButton.Text = "BACK";
    _optionsBackButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
    _optionsBackButton.X = -28f;
    _optionsBackButton.Y = -10f;
    _optionsBackButton.Click += HandleOptionsButtonBack;
    _optionsPanel.AddChild(_optionsBackButton);
}