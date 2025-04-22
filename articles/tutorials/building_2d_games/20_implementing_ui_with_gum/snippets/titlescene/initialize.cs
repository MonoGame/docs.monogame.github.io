public override void Initialize()
{
    // LoadContent is called during base.Initialize().
    base.Initialize();

    // While on the title screen, we can enable exit on escape so the player
    // can close the game by pressing the escape key.
    Core.ExitOnEscape = true;

    // Set the position and origin for the Dungeon text.
    Vector2 size = _font5x.MeasureString(DUNGEON_TEXT);
    _dungeonTextPos = new Vector2(640, 100);
    _dungeonTextOrigin = size * 0.5f;

    // Set the position and origin for the Slime text.
    size = _font5x.MeasureString(SLIME_TEXT);
    _slimeTextPos = new Vector2(757, 207);
    _slimeTextOrigin = size * 0.5f;

    // Initialize the offset of the background pattern at zero
    _backgroundOffset = Vector2.Zero;

    // Set the background pattern destination rectangle to fill the entire
    // screen background
    _backgroundDestination = Core.GraphicsDevice.PresentationParameters.Bounds;

    InitializeUI();
}