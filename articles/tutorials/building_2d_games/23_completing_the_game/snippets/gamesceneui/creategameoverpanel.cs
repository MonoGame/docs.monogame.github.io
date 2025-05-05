private Panel CreateGameOverPanel(TextureAtlas atlas)
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
    text.Text = "GAME OVER";
    text.WidthUnits = DimensionUnitType.RelativeToChildren;
    text.UseCustomFont = true;
    text.CustomFontFile = "fonts/04b_30.fnt";
    text.FontScale = 0.5f;
    text.X = 10.0f;
    text.Y = 10.0f;
    panel.AddChild(text);

    _retryButton = new AnimatedButton(atlas);
    _retryButton.Text = "RETRY";
    _retryButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
    _retryButton.Visual.X = 9.0f;
    _retryButton.Visual.Y = -9.0f;

    _retryButton.Click += OnRetryButtonClicked;
    _retryButton.GotFocus += OnElementGotFocus;

    panel.AddChild(_retryButton);

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