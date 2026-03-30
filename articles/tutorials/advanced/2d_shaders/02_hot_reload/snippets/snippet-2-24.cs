public override void Update(GameTime gameTime)
{
    // Update the grayscale effect if it was changed  
    Content.TryRefresh(_grayscaleEffect);

    // Ensure the UI is always updated
    _ui.Update(gameTime);
