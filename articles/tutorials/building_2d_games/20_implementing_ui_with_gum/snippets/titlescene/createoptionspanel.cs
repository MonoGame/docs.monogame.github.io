private void CreateOptionsPanel() {
    _optionsPanel = new Panel();
    _optionsPanel.Dock(Gum.Wireframe.Dock.Fill);
    _optionsPanel.IsVisible = false;
    _optionsPanel.AddToRoot();

    var optionsText = new TextRuntime();
    optionsText.X = 10;
    optionsText.Y = 10;
    optionsText.Text = "OPTIONS";
    _optionsPanel.AddChild(optionsText);

    var musicLabel = new Label();
    musicLabel.Text = "Music";
    musicLabel.X = 35;
    musicLabel.Y = 35;
    optionsPanel.AddChild(musicLabel);

    var musicSlider = new Slider();
    musicSlider.Anchor(Gum.Wireframe.Anchor.Top);
    musicSlider.Y = 30f;
    musicSlider.Minimum = 0;
    musicSlider.Maximum = 1;
    musicSlider.Value = Core.Audio.SongVolume;
    musicSlider.SmallChange = .1;
    musicSlider.LargeChange = .2;
    musicSlider.ValueChanged += HandleMusicSliderValueChanged;
    musicSlider.ValueChangeCompleted += HandleMusicSliderValueChangeCompleted;
    _optionsPanel.AddChild(musicSlider);

    var sfxLabel = new Label();
    sfxLabel.Text = "SFX";
    sfxLabel.X = 20;
    sfxLabel.Y = 80;
    optionsPanel.AddChild(sfxLabel);

    var sfxSlider = new Slider();
    sfxSlider.Anchor(Gum.Wireframe.Anchor.Top);
    sfxSlider.Y = 93;
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
    _optionsBackButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
    _optionsBackButton.X = -28f;
    _optionsBackButton.Y = -10f;
    _optionsBackButton.Click += HandleOptionsButtonBack;
    _optionsPanel.AddChild(_optionsBackButton);
}
