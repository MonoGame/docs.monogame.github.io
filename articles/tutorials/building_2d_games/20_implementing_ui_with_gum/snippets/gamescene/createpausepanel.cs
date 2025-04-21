private void CreatePausePanel()
{
    _pausePanel = new Panel();
    _pausePanel.Visual.Anchor(Gum.Wireframe.Anchor.Center);
    _pausePanel.Visual.Height = 70;
    _pausePanel.Visual.Width = 264;
    _pausePanel.IsVisible = false;
    _pausePanel.AddToRoot();

    var background = new ColoredRectangleRuntime();
    background.Dock(Dock.Fill);
    background.Color = Color.DarkBlue;
    _pausePanel.AddChild(background);

    var textInstance = new TextRuntime();
    textInstance.Text = "PAUSED";
    textInstance.X = 10f;
    textInstance.Y = 10f;
    _pausePanel.AddChild(textInstance);

    _resumeButton = new Button();
    _resumeButton.Text = "RESUME";
    _resumeButton.Visual.Anchor(Anchor.BottomLeft);
    _resumeButton.Visual.X = 9f;
    _resumeButton.Visual.Y = -9f;
    _resumeButton.Visual.Width = 80;
    _resumeButton.Click += (sender, args) => _pausePanel.IsVisible = false;
    _pausePanel.AddChild(_resumeButton);

    var quitButton = new Button();
    quitButton.Text = "QUIT";
    quitButton.Visual.Anchor(Anchor.BottomRight);
    quitButton.Visual.X = -9f;
    quitButton.Visual.Y = -9f;
    quitButton.Width = 80;
    quitButton.Click += (sender, args) => Core.ChangeScene(new TitleScene());
    _pausePanel.AddChild(quitButton);
}