private void CreateTitlePanel()
{
    // Create a container to hold all of our buttons
    _titleScreenButtonsPanel = new Panel();
    _titleScreenButtonsPanel.Dock(Gum.Wireframe.Dock.Fill);
    _titleScreenButtonsPanel.AddToRoot();

    AnimatedButton startButton = new AnimatedButton(_atlas);
    startButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
    startButton.Visual.X = 50;
    startButton.Visual.Y = -12;
    startButton.Text = "Start";
    startButton.Click += HandleStartClicked;
    _titleScreenButtonsPanel.AddChild(startButton);

    _optionsButton = new AnimatedButton(_atlas);
    _optionsButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
    _optionsButton.Visual.X = -50;
    _optionsButton.Visual.Y = -12;
    _optionsButton.Text = "Options";
    _optionsButton.Click += HandleOptionsClicked;
    _titleScreenButtonsPanel.AddChild(_optionsButton);

    startButton.IsFocused = true;
}