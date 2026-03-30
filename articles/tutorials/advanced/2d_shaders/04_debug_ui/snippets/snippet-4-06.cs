protected override void Draw(GameTime gameTime)
{
    // ...

    // Draw debug UI
    Core.ImGuiRenderer.BeforeLayout(gameTime);  
    
    ImGui.Begin("Demo Window");  
    ImGui.Text("Hello world!");  
    ImGui.End();

    // Finish drawing the debug UI here
    Core.ImGuiRenderer.AfterLayout();

    base.Draw(gameTime);
}