protected override void Initialize()
{
    //...

    // Create the ImGui renderer.
    ImGuiRenderer = new ImGuiRenderer(this);
    ImGuiRenderer.RebuildFontAtlas();

    // Optional: Scale text and widgets for easier readability.
    var io = ImGui.GetIO();
    io.FontGlobalScale = 1.75f;
    ImGui.GetStyle().ScaleAllSizes(1.5f);
}
