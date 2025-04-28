private void UpdateSlimeMovement()
{
    // Get the head of the chain
    SlimeSegment previousHead = _slimes[0];

    // Calculate the new head position
    SlimeSegment newHead = new SlimeSegment();
    newHead.Direction = _nextDirection;
    newHead.At = previousHead.To;
    newHead.To = newHead.At + newHead.Direction * _tilemap.TileWidth;

    // Add the new head and remove the last segment.
    // This effectively move the entire chain forward
    _slimes.Insert(0, newHead);
    _slimes.RemoveAt(_slimes.Count - 1);

    // Now that the slime has moved, ensure it is within the room bounds
    if (!_roomBounds.Contains(newHead.To))
    {
        // It has moved outside the room bounds (collided with a wall) so
        // it is game over
        _gameOverMenu.IsEnabled = _gameOverMenu.IsVisible = _gameOverMenu.IsSelected = true;
    }

    // Next check if the slime is colliding with its own body.
    Circle headBounds = new Circle(
        (int)(newHead.At.X + _slime.Width * 0.5f),
        (int)(newHead.At.Y + _slime.Height * 0.5f),
        (int)(_slime.Width * 0.5f)
    );

    foreach (SlimeSegment child in _slimes[1..])
    {
        Circle childBounds = new Circle(
            (int)(child.At.X + _slime.Width * 0.5f),
            (int)(child.At.Y + _slime.Height * 0.5f),
            (int)(_slime.Width * 0.5f)
        );

        if (headBounds.Intersects(childBounds))
        {
            // The head is colliding with a body segment, so it is game over.
            _gameOverMenu.IsEnabled = _gameOverMenu.IsVisible = _gameOverMenu.IsSelected = true;
        }
    }
}
