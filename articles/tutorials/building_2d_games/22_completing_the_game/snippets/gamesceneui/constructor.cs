public GameSceneUI()
{
    // The game scene UI inherits from ContainerRuntime, so we set its
    // doc to fill so it fills the entire screen.
    Dock(Gum.Wireframe.Dock.Fill);

    // Add it to the root element.
    this.AddToRoot();

    // Get a reference to the content manager that was registered with the
    // GumService when it was original initialized.
    ContentManager content = GumService.Default.ContentLoader.XnaContentManager;

    // Use that content manager to load the sound effect and atlas for the
    // user interface elements
    _uiSoundEffect = content.Load<SoundEffect>("audio/ui");
    TextureAtlas atlas = TextureAtlas.FromFile(content, "images/atlas-definition.xml");

    // Create the text that will display the players score and add it as
    // a child to this container.
    _scoreText = CreateScoreText();
    AddChild(_scoreText);

    // Create the background overlay that fills the container with a
    // transparent blue background when the pause panel or game over
    // panel is shown to visually take focus away from the active game and
    // add it as a child to this container.
    _overlay = CreateOverlay();
    AddChild(_overlay);

    // Create the Pause panel that is displayed when the game is paused and
    // add it as a child to this container
    _pausePanel = CreatePausePanel(atlas);
    AddChild(_pausePanel.Visual);

    // Create the Game Over panel that is displayed when a game over occurs
    // and add it as a child to this container
    _gameOverPanel = CreateGameOverPanel(atlas);
    AddChild(_gameOverPanel.Visual);
}