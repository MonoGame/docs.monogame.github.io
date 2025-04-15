private void MoveBat()
{
    // Calculate a new position for the bat
    Vector2 newPosition = _batPosition + _batVelocity;

    // Create the bounds for the bat
    Circle batBounds = new Circle(
        (int)(newPosition.X + _bat.Width * 0.5f),
        (int)(newPosition.Y + _bat.Height * 0.5f),
        (int)(_bat.Width * 0.25f)
    );

    // Use distance based checks to determine of the bat is within the
    // bounds of the game screen, and if it is outside that screen edge,
    // reflect it about the screen edge normal.
    Vector2 normal = Vector2.Zero;

    if (batBounds.Left < _roomBounds.Left)
    {
        normal.X = Vector2.UnitX.X;
        newPosition.X = _roomBounds.Left;
    }
    else if (batBounds.Right > _roomBounds.Right)
    {
        normal.X = -Vector2.UnitX.X;
        newPosition.X = _roomBounds.Right - _bat.Width;
    }

    if (batBounds.Top < _roomBounds.Top)
    {
        normal.Y = Vector2.UnitY.Y;
        newPosition.Y = _roomBounds.Top;
    }
    else if (batBounds.Bottom > _roomBounds.Bottom)
    {
        normal.Y = -Vector2.UnitY.Y;
        newPosition.Y = _roomBounds.Bottom - _bat.Height;
    }

    // If the normal is anything but Vector2.Zero, this means the bat had
    // moved outside the screen edge so we should reflect it about the
    // normal.
    if (normal != Vector2.Zero)
    {
        _batVelocity = Vector2.Reflect(_batVelocity, normal);

        // Play the bounce sound effect
        Core.Audio.PlaySoundEffect(_bounceSoundEffect);
    }

    _batPosition = newPosition;
}
