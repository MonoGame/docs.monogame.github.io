public override void Update(GameTime gameTime)
{
    // Ensure the UI is always updated
    _ui.Update(gameTime);

    // Update the grayscale effect if it was changed
    _grayscaleEffect.Update();
