public override void Initialize()
{
    // LoadContent is called during base.Initialize().
    base.Initialize();

    // During the game scene, we want to disable exit on escape. Instead,
    // the escape key will be used to return back to the title screen
    Core.ExitOnEscape = false;

    Rectangle screenBounds = Core.GraphicsDevice.PresentationParameters.Bounds;

    _roomBounds = new Rectangle(
        (int)_tilemap.TileWidth,
        (int)_tilemap.TileHeight,
        screenBounds.Width - (int)_tilemap.TileWidth * 2,
        screenBounds.Height - (int)_tilemap.TileHeight * 2
    );

    // Set the position of the score text to align to the left edge of the
    // room bounds.
    _scoreTextPosition = new Vector2(_roomBounds.Left, _tilemap.TileHeight * 0.5f);

    // Set the origin of the text so it is centered horizontally
    float scoreTextYOrigin = _font.MeasureString("Score").Y * 0.5f;
    _scoreTextOrigin = new Vector2(0, scoreTextYOrigin);

    // Initialize the game controller
    _controller = new GameController();

    InitializeNewGame();
}
