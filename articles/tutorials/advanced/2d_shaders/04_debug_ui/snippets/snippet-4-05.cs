protected override void Draw(GameTime gameTime)
{
    // If there is an active scene, draw it.
    if (s_activeScene != null)
    {
        s_activeScene.Draw(gameTime);
    }

    // Draw debug UI
    Core.ImGuiRenderer.BeforeLayout(gameTime);
    // Finish drawing the debug UI here
    Core.ImGuiRenderer.AfterLayout();

    base.Draw(gameTime);
}