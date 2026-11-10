/// <summary>
/// Randomizes the velocity of the bat.
/// </summary>
public void RandomizeVelocity()
{
    // Generate a random angle
    float angle = (float)(Random.Shared.NextDouble() * MathHelper.TwoPi);

    // Convert the angle to a direction vector
    float x = (float)Math.Cos(angle);
    float y = (float)Math.Sin(angle);
    Vector2 direction = new Vector2(x, y);

    // Multiply the direction vector by the movement speed to get the
    // final velocity
    _velocity = direction * MOVEMENT_SPEED;
}