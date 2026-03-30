protected override void Initialize()
{
    base.Initialize();

    // Set the core's graphics device to a reference of the base Game's
    // graphics device.
    GraphicsDevice = base.GraphicsDevice;

    // Create the sprite batch instance.
    SpriteBatch = new SpriteBatch(GraphicsDevice);

    // Create a new input manager
    Input = new InputManager();

    // Create a new audio controller.
    Audio = new AudioController();

    // Create the ImGui renderer.
    ImGuiRenderer = new ImGuiRenderer(this);
    ImGuiRenderer.RebuildFontAtlas();
}