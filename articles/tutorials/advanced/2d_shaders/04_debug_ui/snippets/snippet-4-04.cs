protected override void Initialize()
{
    base.Initialize();

    // ... 

    // Create the ImGui renderer.
    ImGuiRenderer = new ImGuiRenderer(this);
    ImGuiRenderer.RebuildFontAtlas();

    // ...
}