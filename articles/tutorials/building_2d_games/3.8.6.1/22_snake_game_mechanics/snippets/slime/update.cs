/// <summary>
/// Updates the slime.
/// </summary>
/// <param name="gameTime">A snapshot of the timing values for the current update cycle.</param>
public void Update(GameTime gameTime)
{
    // Update the animated sprite.
    _sprite.Update(gameTime);

    // Handle any player input
    HandleInput();

    // Increment the movement timer by the frame elapsed time.
    _movementTimer += gameTime.ElapsedGameTime;

    // If the movement timer has accumulated enough time to be greater than
    // the movement time threshold, then perform a full movement.
    if (_movementTimer >= s_movementTime)
    {
        _movementTimer -= s_movementTime;
        Move();
    }

    // Update the movement lerp offset amount
    _movementProgress = (float)(_movementTimer.TotalSeconds / s_movementTime.TotalSeconds);
}