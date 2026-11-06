    public override void Update(GameTime gameTime)
    {
        // Ensure the UI is always updated
        Gum.GumService.Default.Update(gameTime);

        // If the game is paused, do not continue
        if (_pausePanel.IsVisible)
        {
            return;
        }

        // Existing game update code
        // ...