private void InitializeNewGame()
{
    // Calculate the position for the slime, which will be at the center
    // tile of the tile map.
    Vector2 slimePos = new Vector2();
    slimePos.X = (_tilemap.Columns / 2) * _tilemap.TileWidth;
    slimePos.Y = (_tilemap.Rows / 2) * _tilemap.TileHeight;

    // Initialize the slime.
    _slime.Initialize(slimePos, _tilemap.TileWidth);

    // Initialize the bat.
    _bat.RandomizeVelocity();
    PositionBatAwayFromSlime();

    // Reset the score.
    _score = 0;

    // Set the game state to playing.
    _state = GameState.Playing;
}