private void PositionBatAwayFromSlime()
{
    // Calculate the position that is in the center of the bounds
    // of the room.
    float roomCenterX = _roomBounds.X + _roomBounds.Width * 0.5f;
    float roomCenterY = _roomBounds.Y + _roomBounds.Height * 0.5f;
    Vector2 roomCenter = new Vector2(roomCenterX, roomCenterY);

    // Get the bounds of the slime and calculate the center position.
    Circle slimeBounds = _slime.GetBounds();
    Vector2 slimeCenter = new Vector2(slimeBounds.X, slimeBounds.Y);

    // Calculate the distance vector from the center of the room to the
    // center of the slime.
    Vector2 centerToSlime = slimeCenter - roomCenter;

    // Get the bounds of the bat.
    Circle batBounds =_bat.GetBounds();

    // Calculate the amount of padding we will add to the new position of
    // the bat to ensure it is not sticking to walls
    int padding = batBounds.Radius * 2;

    // Calculate the new position of the bat by finding which component of
    // the center to slime vector (X or Y) is larger and in which direction.
    Vector2 newBatPosition = Vector2.Zero;
    if (Math.Abs(centerToSlime.X) > Math.Abs(centerToSlime.Y))
    {
        // The slime is closer to either the left or right wall, so the Y
        // position will be a random position between the top and bottom
        // walls.
        newBatPosition.Y = Random.Shared.Next(
            _roomBounds.Top + padding,
            _roomBounds.Bottom - padding
        );

        if (centerToSlime.X > 0)
        {
            // The slime is closer to the right side wall, so place the
            // bat on the left side wall.
            newBatPosition.X = _roomBounds.Left + padding;
        }
        else
        {
            // The slime is closer ot the left side wall, so place the
            // bat on the right side wall.
            newBatPosition.X = _roomBounds.Right - padding * 2;
        }
    }
    else
    {
        // The slime is closer to either the top or bottom wall, so the X
        // position will be a random position between the left and right
        // walls.
        newBatPosition.X = Random.Shared.Next(
            _roomBounds.Left + padding,
            _roomBounds.Right - padding
        );

        if (centerToSlime.Y > 0)
        {
            // The slime is closer to the top wall, so place the bat on the
            // bottom wall.
            newBatPosition.Y = _roomBounds.Top + padding;
        }
        else
        {
            // The slime is closer to the bottom wall, so place the bat on
            // the top wall.
            newBatPosition.Y = _roomBounds.Bottom - padding * 2;
        }
    }

    // Assign the new bat position.
    _bat.Position = newBatPosition;
}