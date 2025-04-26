private void InitializeGum()
{
    // Initialize the Gum service
    GumService.Default.Initialize(this);

    // Tell the Gum service which content manager to use.  We'll tell it to
    // use the global content manager from our Core.
    GumService.Default.ContentLoader.XnaContentManager = Core.Content;

    // Register keyboard input for UI control.
    FrameworkElement.KeyboardsForUiControl.Add(GumService.Default.Keyboard);

    // Register gamepad input for Ui control.
    FrameworkElement.GamePadsForUiControl.AddRange(GumService.Default.Gamepads);

    // Customize the tab reverse UI navigation to also trigger when the keyboard
    // Up arrow key is pushed.
    FrameworkElement.TabReverseKeyCombos.Add(
       new KeyCombo() { PushedKey = Microsoft.Xna.Framework.Input.Keys.Up });

    // Customize the tab UI navigation to also trigger when the keyboard
    // Down arrow key is pushed.
    FrameworkElement.TabKeyCombos.Add(
       new KeyCombo() { PushedKey = Microsoft.Xna.Framework.Input.Keys.Down });

    // The assets created for the UI were done so at 1/4th the size to keep the size of the
    // texture atlas small.  So we'll set the default canvas size to be 1/4th the size of
    // the game's resolution then tell gum to zoom in by a factor of 4.
    GumService.Default.CanvasWidth = GraphicsDevice.PresentationParameters.BackBufferWidth / 4.0f;
    GumService.Default.CanvasHeight = GraphicsDevice.PresentationParameters.BackBufferHeight / 4.0f;
    GumService.Default.Renderer.Camera.Zoom = 4.0f;
}