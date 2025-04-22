private void CreateOptionsPanel()
{
    _optionsPanel = new Panel();
    _optionsPanel.Visual.Dock(Gum.Wireframe.Dock.Fill);
    _optionsPanel.IsVisible = false;
    _optionsPanel.AddToRoot();

    var optionsText = new TextRuntime();
    optionsText.X = 10;
    optionsText.Y = 10;
    optionsText.Text = "OPTIONS";
    _optionsPanel.AddChild(optionsText);

    var musicSlider = new Slider();
    musicSlider.Visual.Anchor(Gum.Wireframe.Anchor.Top);
    musicSlider.Visual.Y = 30f;
    musicSlider.Minimum = 0;
    musicSlider.Maximum = 1;
    musicSlider.Value = Core.Audio.SongVolume;
    musicSlider.SmallChange = .1;
    musicSlider.LargeChange = .2;
    musicSlider.ValueChanged += HandleMusicSliderValueChanged;
    musicSlider.ValueChangeCompleted += HandleMusicSliderValueChangeCompleted;
    _optionsPanel.AddChild(musicSlider);

    var sfxSlider = new Slider();
    sfxSlider.Visual.Anchor(Gum.Wireframe.Anchor.Top);
    sfxSlider.Visual.Y = 93;
    sfxSlider.Minimum = 0;
    sfxSlider.Maximum = 1;
    sfxSlider.Value = Core.Audio.SoundEffectVolume;
    sfxSlider.SmallChange = .1;
    sfxSlider.LargeChange = .2;
    sfxSlider.ValueChanged += HandleSfxSliderChanged;
    sfxSlider.ValueChangeCompleted += HandleSfxSliderChangeCompleted;
    _optionsPanel.AddChild(sfxSlider);

    _optionsBackButton = new Button();
    _optionsBackButton.Text = "BACK";
    _optionsBackButton.Visual.Anchor(Gum.Wireframe.Anchor.BottomRight);
    _optionsBackButton.X = -28f;
    _optionsBackButton.Y = -10f;
    _optionsBackButton.Click += HandleOptionsButtonBack;
    _optionsPanel.AddChild(_optionsBackButton);
}