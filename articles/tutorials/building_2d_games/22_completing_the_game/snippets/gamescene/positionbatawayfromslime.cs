private void PositionBatAwayFromSlime()
{
    // Get the bounds of the slime and the bat.
    Circle slimeBounds = _slime.GetBounds();
    Circle batBounds = _bat.GetBounds();

    // Get the center of the room bounds
    float centerX = _roomBounds.X + _roomBounds.Width * 0.5f;
    float centerY = _roomBounds.Y + _roomBounds.Height * 0.5f;
    Vector2 center = new Vector2(centerX, centerY);

    // Get the center of the slime bounds
    Vector2 slimePos = new Vector2(slimeBounds.X, slimeBounds.Y);

    // Calculate the vector from the center of the room to the slime.
    Vector2 centerToSlime = slimePos - center;

    // Get the current position of the bat so we can adjust it.
    Vector2 newBatPosition = _bat.Position;

    // Determine the furthest wall by finding which component (x or y) is
    // larger and in which direction
    if (Math.Abs(centerToSlime.X) > Math.Abs(centerToSlime.Y))
    {
        // Slime is closer to either the left or right wall, so the Y position
        // will be the center Y position
        newBatPosition.Y = center.Y;

        // Now determine the X position based on which wall the slime is
        // closest to.
        if (centerToSlime.X > 0)
        {
            // Slime is on the right side, place the bat on the left wall
            newBatPosition.X = _roomBounds.Left;
        }
        else
        {
            // Slime is on the left side, place the bat on the right wall
            newBatPosition.X = _roomBounds.Right - batBounds.Radius * 2.0f;
        }
    }
    else
    {
        // Slime is closer to either the top or bottom wall, so the X position
        // will be the center X position
        newBatPosition.X = center.X;

        // Now determine the Y position based on which wall the slime is
        // closest to.
        if (centerToSlime.Y > 0)
        {
            // Slime is closer to the bottom, place the bat on the top wall.
            newBatPosition.Y = _roomBounds.Top;
        }
        else
        {
            // Slime is closer to the top, place the bat on the bottom wall.
            newBatPosition.Y = _roomBounds.Bottom - batBounds.Radius * 2.0f;
        }
    }

    // Assign the new bat position
    _bat.Position = newBatPosition;
}