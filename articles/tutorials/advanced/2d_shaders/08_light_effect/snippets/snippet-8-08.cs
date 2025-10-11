public override void Draw(GameTime gameTime)
{
    // ...

    // Draw the UI
    _ui.Draw();

    // Render the debug view for the game  
    _deferredRenderer.DebugDraw();
}