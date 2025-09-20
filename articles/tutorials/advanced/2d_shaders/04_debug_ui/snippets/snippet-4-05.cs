protected override void Draw(GameTime gameTime)
{
    // ...

    // Draw debug UI
    Core.ImGuiRenderer.BeforeLayout(gameTime);  
    // draw the debug UI here  
    Core.ImGuiRenderer.AfterLayout();

    base.Draw(gameTime);
}