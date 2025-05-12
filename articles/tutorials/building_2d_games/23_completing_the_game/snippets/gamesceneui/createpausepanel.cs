private Panel CreatePausePanel(TextureAtlas atlas)
{
    Panel panel = new Panel();
    panel.Anchor(Gum.Wireframe.Anchor.Center);
    panel.Visual.WidthUnits = DimensionUnitType.Absolute;
    panel.Visual.HeightUnits = DimensionUnitType.Absolute;
    panel.Visual.Width = 264.0f;
    panel.Visual.Height = 70.0f;
    panel.IsVisible = false;

    TextureRegion backgroundRegion = atlas.GetRegion("panel-background");

    NineSliceRuntime background = new NineSliceRuntime();
    background.Dock(Gum.Wireframe.Dock.Fill);
    background.Texture = backgroundRegion.Texture;
    background.TextureAddress = TextureAddress.Custom;
    background.TextureHeight = backgroundRegion.Height;
    background.TextureWidth = backgroundRegion.Width;
    background.TextureTop = backgroundRegion.SourceRectangle.Top;
    background.TextureLeft = backgroundRegion.SourceRectangle.Left;
    panel.AddChild(background);

    TextRuntime text = new TextRuntime();
    text.Text = "PAUSED";
    text.UseCustomFont = true;
    text.CustomFontFile = "fonts/04b_30.fnt";
    text.FontScale = 0.5f;
    text.X = 10.0f;
    text.Y = 10.0f;
    panel.AddChild(text);

    _resumeButton = new AnimatedButton(atlas);
    _resumeButton.Text = "RESUME";
    _resumeButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
    _resumeButton.Visual.X = 9.0f;
    _resumeButton.Visual.Y = -9.0f;

    _resumeButton.Click += OnResumeButtonClicked;
    _resumeButton.GotFocus += OnElementGotFocus;

    panel.AddChild(_resumeButton);

    AnimatedButton quitButton = new AnimatedButton(atlas);
    quitButton.Text = "QUIT";
    quitButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
    quitButton.Visual.X = -9.0f;
    quitButton.Visual.Y = -9.0f;

    quitButton.Click += OnQuitButtonClicked;
    quitButton.GotFocus += OnElementGotFocus;

    panel.AddChild(quitButton);

    return panel;
}