private void UpdateGame(GameTime gameTime)
{
    // Update the slime animated sprite.
    _slime.Update(gameTime);

    // Update the bat animated sprite.
    _bat.Update(gameTime);

    CheckInput();

    // Check if there is a collision between the slime and bat
    CheckSlimeAndBatCollision();

    // Increment the tick timer
    _tickTimer += gameTime.ElapsedGameTime * 2.5f;

    // If the tick timer has exceeded the time to tick, move the slime chain
    if (_tickTimer >= s_tickTime)
    {
        _tickTimer -= s_tickTime;
        UpdateSlimeMovement();
    }

    // Move the bat
    MoveBat();
}
