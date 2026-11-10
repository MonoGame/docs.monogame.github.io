private const float MOVEMENT_SPEED = 5.0f;

// The velocity of the bat that defines the direction and how much in that
// direction to update the bats position each update cycle.
private Vector2 _velocity;

// The AnimatedSprite used when drawing the bat.
private AnimatedSprite _sprite;

// The sound effect to play when the bat bounces off the edge of the room.
private SoundEffect _bounceSoundEffect;