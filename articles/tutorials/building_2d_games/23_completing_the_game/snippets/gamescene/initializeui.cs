private void InitializeUI()
{
    // Clear out any previous UI element incase we came here
    // from a different scene.
    GumService.Default.Root.Children.Clear();

    // Create the game scene ui instance.
    _ui = new GameSceneUI();

    // Subscribe to the events from the game scene ui.
    _ui.ResumeButtonClick += OnResumeButtonClicked;
    _ui.RetryButtonClick += OnRetryButtonClicked;
    _ui.QuitButtonClick += OnQuitButtonClicked;
}