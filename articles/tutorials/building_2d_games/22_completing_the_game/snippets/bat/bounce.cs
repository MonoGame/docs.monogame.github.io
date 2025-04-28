/// <summary>
/// Handles a bounce event when the bat collides with a wall or boundary.
/// </summary>
/// <param name="normal">The normal vector of the surface the bat is bouncing against.</param>
public void Bounce(Vector2 normal)
{
    Vector2 newPosition = Position;

    // Adjust the position based on the normal to prevent sticking to walls.
    if(normal.X != 0)
    {
        // We are bouncing off a vertical wall (left/right).
        // Move slightly away from the wall in the direction of the normal.
        newPosition.X += normal.X * (_sprite.Width * 0.1f);
    }

    if(normal.Y != 0)
    {
        // We are bouncing off a horizontal wall (top/bottom).
        // Move slightly way from the wall in the direction of the normal.
        newPosition.Y += normal.Y * (_sprite.Height * 0.1f);
    }

    // Apply the new position
    Position = newPosition;

    // Apply reflection based on the normal.
    _velocity = Vector2.Reflect(_velocity, normal);

    // Play the bounce sound effect.
    Core.Audio.PlaySoundEffect(_bounceSoundEffect);
}