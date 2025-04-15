private void InitializeNewGame()
{
    // Create the list of slime segments
    _slimes = new List<SlimeSegment>();

    // Initial slime position will be the center tile of the tile map.
    int centerRow = _tilemap.Rows / 2;
    int centerColumn = _tilemap.Columns / 2;

    // Create the initial head segment.
    SlimeSegment segment = new SlimeSegment();
    segment.At = new Vector2(centerColumn * _tilemap.TileWidth, centerRow * _tilemap.TileHeight);
    segment.To = segment.At + new Vector2(_tilemap.TileWidth, 0);
    segment.Direction = Vector2.UnitX;
    _slimes.Add(segment);

    // Set the initial direction to match the head
    _nextDirection = segment.Direction;

    // Initialize the bat position to a random position
    AssignRandomBatPosition();

    // Assign the initial random position and velocity to the bat
    AssignRandomBatVelocity();

    // Reset the timer and score
    _tickTimer = s_tickTime;
    _score = 0;
}
