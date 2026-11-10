// Calculate the new position of the ball based on the velocity.
Vector2 newPosition = _ballPosition + _ballVelocity;

// Get the bounds of the ball as a rectangle.
Rectangle ballBounds = new Rectangle(
    (int)_ballPosition.X,
    (int)_ballPosition.Y,
    (int)_ball.Width,
    (int)_ball.Height
);

// Get the bounds of the screen as a rectangle.
Rectangle screenBounds = new Rectangle(
    0,
    0,
    GraphicsDevice.PresentationParameters.BackBufferWidth,
    GraphicsDevice.PresentationParameters.BackBufferHeight
);

// Detect if the ball object is within the screen bounds.
if(!screenBounds.Contains(ballBounds))
{
    // Ball would move outside the screen
    // First find the distance from the edge of the ball to each edge of the screen.
    float distanceLeft = Math.Abs(screenBounds.Left - ballBounds.Left);
    float distanceRight = Math.Abs(screenBounds.Right - ballBounds.Right);
    float distanceTop = Math.Abs(screenBounds.Top - ballBounds.Top);
    float distanceBottom = Math.Abs(screenBounds.Bottom - ballBounds.Bottom);

    // Determine which screen edge is the closest.
    float minDistance = Math.Min(
        Math.Min(distanceLeft, distanceRight),
        Math.Min(distanceTop, distanceBottom)
    );

    // Determine the normal vector based on which screen edge is the closest.
    Vector2 normal;
    if (minDistance == distanceLeft)
    {
        // Closest to the left edge.
        normal = Vector2.UnitX;
        newPosition.X = 0;
    }
    else if (minDistance == distanceRight)
    {
        // Closest to the right edge.
        normal = -Vector2.UnitX;
        newPosition.X = screenBounds.Right - _ball.Width;
    }
    else if (minDistance == distanceTop)
    {
        // Closest to the top edge.
        normal = Vector2.UnitY;
        newPosition.Y = 0;
    }
    else
    {
        // Closest to the bottom edge.
        normal = -Vector2.UnitY;
        newPosition.Y = screenBounds.Bottom - _ball.Height;
    }

    // Reflect the velocity about the normal.
    _ballVelocity = Vector2.Reflect(_ballVelocity, normal); 
}

// Set the new position of the ball.
_ballVelocity = newPosition;