public override void Draw(GameTime gameTime)
{
    // Existing game draw code
    // ...

    // Always end the sprite batch when finished
    Core.SpriteBatch.End();
    
    // Draw the Gum UI
    GumService.Default.Draw();
}