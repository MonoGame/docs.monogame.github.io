private void CreateTitlePanel()
{
    // Create a container to hold all of our buttons
    _titleScreenButtonsPanel = new Panel();
    _titleScreenButtonsPanel.Visual.Dock(Gum.Wireframe.Dock.Fill);
    _titleScreenButtonsPanel.AddToRoot();

    var startButton = new Button();
    startButton.Visual.Anchor(Gum.Wireframe.Anchor.BottomLeft);
    startButton.Visual.X = 50;
    startButton.Visual.Y = -12;
    startButton.Visual.Width = 70;
    startButton.Text = "Start";
    startButton.Click += HandleStartClicked;
    _titleScreenButtonsPanel.AddChild(startButton);

    _optionsButton = new Button();
    _optionsButton.Visual.Anchor(Gum.Wireframe.Anchor.BottomRight);
    _optionsButton.Visual.X = -50;
    _optionsButton.Visual.Y = -12;
    _optionsButton.Visual.Width = 70;
    _optionsButton.Text = "Options";
    _optionsButton.Click += HandleOptionsClicked;
    _titleScreenButtonsPanel.AddChild(_optionsButton);

    startButton.IsFocused = true;
}