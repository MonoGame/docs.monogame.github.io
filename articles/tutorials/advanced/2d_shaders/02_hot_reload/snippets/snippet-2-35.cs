public override void Update(GameTime gameTime)
{
    // Update the grayscale effect if it was changed  
    _grayscaleEffect.TryRefresh(out _);

    // Ensure the UI is always updated
    _ui.Update(gameTime);
