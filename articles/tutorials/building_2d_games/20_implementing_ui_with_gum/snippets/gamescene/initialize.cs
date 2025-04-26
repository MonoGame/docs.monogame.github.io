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

    // Initial slime position will be the center tile of the tile map.
    int centerRow = _tilemap.Rows / 2;
    int centerColumn = _tilemap.Columns / 2;
    _slimePosition = new Vector2(centerColumn * _tilemap.TileWidth, centerRow * _tilemap.TileHeight);

    // Initial bat position will the in the top left corner of the room
    _batPosition = new Vector2(_roomBounds.Left, _roomBounds.Top);

    // Set the position of the score text to align to the left edge of the
    // room bounds, and to vertically be at the center of the first tile.
    _scoreTextPosition = new Vector2(_roomBounds.Left, _tilemap.TileHeight * 0.5f);

    // Set the origin of the text so it's left-centered.
    float scoreTextYOrigin = _font.MeasureString("Score").Y * 0.5f;
    _scoreTextOrigin = new Vector2(0, scoreTextYOrigin);

    // Assign the initial random velocity to the bat.
    AssignRandomBatVelocity();

    InitializeUI();
}