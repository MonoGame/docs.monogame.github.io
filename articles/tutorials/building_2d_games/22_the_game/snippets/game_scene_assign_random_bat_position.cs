private void AssignRandomBatPosition()
{
    // Get a reference to the snake's head
    SlimeSegment head = _slimes[0];

    // Calculate the center of the room
    Vector2 roomCenter = new Vector2(
        _roomBounds.X + _roomBounds.Width * 0.5f,
        _roomBounds.Y + _roomBounds.Height * 0.5f
    );

    // Calculate the vector from the room center to the head
    Vector2 centerToHead = head.At - roomCenter;

    // Determine the furthest wall by finding which component (x or y) is
    // larger and in which direction
    if (Math.Abs(centerToHead.X) > Math.Abs(centerToHead.Y))
    {
        // Head is closer to either the left or right wall
        if (centerToHead.X > 0)
        {
            // Head is on the right side, place bat on the left wall
            _batPosition = new Vector2(
                _roomBounds.Left + _bat.Width,
                roomCenter.Y
            );
        }
        else
        {
            // Head is on left side, place bat on the right wall
            _batPosition = new Vector2(
                _roomBounds.Right - _bat.Width * 2,
                roomCenter.Y
            );
        }
    }
    else
    {
        // Head is closer to top or bottom wall
        if (centerToHead.Y > 0)
        {
            // Head is closer to bottom, place bat on top wall
            _batPosition = new Vector2(
                roomCenter.X,
                _roomBounds.Top + _bat.Height
            );
        }
        else
        {
            // Head is closer to top, place bat on bottom wall.
            _batPosition = new Vector2(
                roomCenter.X,
                _roomBounds.Bottom - _bat.Height * 2
            );
        }
    }
}
