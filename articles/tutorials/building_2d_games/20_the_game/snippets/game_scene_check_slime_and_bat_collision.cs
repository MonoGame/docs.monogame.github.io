private void CheckSlimeAndBatCollision()
{
    // Get a reference to the head of the slime
    SlimeSegment head = _slimes[0];

    // Create the bounds for the head of the slime
    Circle slimeBounds = new Circle(
        (int)(head.At.X + _slime.Width * 0.5f),
        (int)(head.At.Y + _slime.Height * 0.5f),
        (int)(_slime.Width * 0.5f)
    );

    // Create the bounds for the bat
    Circle batBounds = new Circle(
        (int)(_batPosition.X + _bat.Width * 0.5f),
        (int)(_batPosition.Y + _bat.Height * 0.5f),
        (int)(_bat.Width * 0.25f)
    );

    // If the slime and bat are intersecting, then eat the bat and create
    // a new slime segment
    if (slimeBounds.Intersects(batBounds))
    {
        // Get the tail segment
        SlimeSegment tail = _slimes[_slimes.Count - 1];

        // Create a new tail segment behind the current tail
        SlimeSegment newTail = new SlimeSegment();
        newTail.At = tail.To + tail.ReverseDirection * _tilemap.TileWidth;
        newTail.To = tail.At;
        newTail.Direction = Vector2.Normalize(tail.At - newTail.At);

        // Add the new segment to the end of the chain
        _slimes.Add(newTail);

        // Assign the bat position to a random position
        AssignRandomBatPosition();

        // Assign a new random velocity to the bat.
        AssignRandomBatVelocity();

        // Play the collect sound effect
        Core.Audio.PlaySoundEffect(_collectSoundEffect);

        // Increase the player's score
        _score += 100;
    }
}
