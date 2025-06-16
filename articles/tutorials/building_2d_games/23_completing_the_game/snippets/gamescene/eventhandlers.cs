private void OnResumeButtonClicked(object sender, EventArgs args)
{
    // Change the game state back to playing.
    _state = GameState.Playing;
}

private void OnRetryButtonClicked(object sender, EventArgs args)
{
    // Player has chosen to retry, so initialize a new game.
    InitializeNewGame();
}

private void OnQuitButtonClicked(object sender, EventArgs args)
{
    // Player has chosen to quit, so return back to the title scene.
    Core.ChangeScene(new TitleScene());
}