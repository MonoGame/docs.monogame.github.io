private void CollisionChecks()
{
    // Capture the current bounds of the slime and bat.
    Circle slimeBounds = _slime.GetBounds();
    Circle batBounds = _bat.GetBounds();

    // FIrst perform a collision check to see if the slime is colliding with
    // the bat, which means the slime eats the bat.
    if (slimeBounds.Intersects(batBounds))
    {
        // Move the bat to a new position away from the slime.
        PositionBatAwayFromSlime();

        // Randomize the velocity of the bat.
        _bat.RandomizeVelocity();

        // Tell the slime to grow.
        _slime.Grow();

        // Increment the score.
        _score += 100;

        // Update the score display on the UI.
        _ui.UpdateScoreText(_score);

        // Play the collect sound effect.
        Core.Audio.PlaySoundEffect(_collectSoundEffect);
    }

    // Next check if the slime is colliding with the wall by validating if
    // it is within the bounds of the room.  If it is outside the room
    // bounds, then it collided with a wall which triggers a game over.
    if (slimeBounds.Top < _roomBounds.Top ||
       slimeBounds.Bottom > _roomBounds.Bottom ||
       slimeBounds.Left < _roomBounds.Left ||
       slimeBounds.Right > _roomBounds.Right)
    {
        GameOver();
        return;
    }

    // Finally, check if the bat is colliding with a wall by validating if
    // it is within the bounds of the room.  If it is outside the room
    // bounds, then it collided with a wall, and the bat should bounce
    // off of that wall.
    if (batBounds.Top < _roomBounds.Top)
    {
        _bat.Bounce(Vector2.UnitY);
    }
    else if (batBounds.Bottom > _roomBounds.Bottom)
    {
        _bat.Bounce(-Vector2.UnitY);
    }

    if (batBounds.Left < _roomBounds.Left)
    {
        _bat.Bounce(Vector2.UnitX);
    }
    else if (batBounds.Right > _roomBounds.Right)
    {
        _bat.Bounce(-Vector2.UnitX);
    }
}