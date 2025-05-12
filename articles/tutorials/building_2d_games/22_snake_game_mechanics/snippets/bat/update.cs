/// <summary>
/// Updates the bat.
/// </summary>
/// <param name="gameTime">A snapshot of the timing values for the current update cycle.</param>
public void Update(GameTime gameTime)
{
    // Update the animated sprite
    _sprite.Update(gameTime);

    // Update the position of the bat based on the velocity.
    Position += _velocity;
}