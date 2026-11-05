/// <summary>
/// Creates a new Bat using the specified animated sprite and sound effect.
/// </summary>
/// <param name="sprite">The AnimatedSprite ot use when drawing the bat.</param>
/// <param name="bounceSoundEffect">The sound effect to play when the bat bounces off a wall.</param>
public Bat(AnimatedSprite sprite, SoundEffect bounceSoundEffect)
{
    _sprite = sprite;
    _bounceSoundEffect = bounceSoundEffect;
}