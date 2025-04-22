private void CreatePausePanel()
{
    _pausePanel = new Panel();
    _pausePanel.Visual.Anchor(Anchor.Center);
    _pausePanel.Visual.WidthUnits = DimensionUnitType.Absolute;
    _pausePanel.Visual.HeightUnits = DimensionUnitType.Absolute;
    _pausePanel.Visual.Height = 70;
    _pausePanel.Visual.Width = 264;
    _pausePanel.IsVisible = false;
    _pausePanel.AddToRoot();

    TextureRegion backgroundRegion = _atlas.GetRegion("panel-background");

    NineSliceRuntime background = new NineSliceRuntime();
    background.Dock(Dock.Fill);
    background.Texture = backgroundRegion.Texture;
    background.TextureAddress = TextureAddress.Custom;
    background.TextureHeight = backgroundRegion.Height;
    background.TextureLeft = backgroundRegion.SourceRectangle.Left;
    background.TextureTop = backgroundRegion.SourceRectangle.Top;
    background.TextureWidth = backgroundRegion.Width;
    _pausePanel.AddChild(background);

    TextRuntime textInstance = new TextRuntime();
    textInstance.Text = "PAUSED";
    textInstance.CustomFontFile = @"fonts/04b_30.fnt";
    textInstance.UseCustomFont = true;
    textInstance.FontScale = 0.5f;
    textInstance.X = 10f;
    textInstance.Y = 10f;
    _pausePanel.AddChild(textInstance);

    _resumeButton = new AnimatedButton(_atlas);
    _resumeButton.Text = "RESUME";
    _resumeButton.Visual.Anchor(Anchor.BottomLeft);
    _resumeButton.Visual.X = 9f;
    _resumeButton.Visual.Y = -9f;
    _resumeButton.Click += HandleResumeButtonClicked;
    _pausePanel.AddChild(_resumeButton);

    AnimatedButton quitButton = new AnimatedButton(_atlas);
    quitButton.Text = "QUIT";
    quitButton.Visual.Anchor(Anchor.BottomRight);
    quitButton.Visual.X = -9f;
    quitButton.Visual.Y = -9f;
    quitButton.Click += HandleQuitButtonClicked;

    _pausePanel.AddChild(quitButton);
}