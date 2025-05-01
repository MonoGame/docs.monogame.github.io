// A constant value that represents the amount of time to wait between
// movement updates.
private static readonly TimeSpan s_movementTime = TimeSpan.FromMilliseconds(10000);

// The amount of time that has elapsed since the last movement update.
private TimeSpan _movementTimer;

// Normalized value (0-1) representing progress between movement ticks for visual interpolation
private float _movementProgress;

// The next direction to apply to the head of the slime chain during the
// next movement update.
private Vector2 _nextDirection;

// The number of pixels to move the head segment during the movement cycle.
private float _stride;

// Tracks the segments of the slime chain.
private List<SlimeSegment> _segments;

// The AnimatedSprite used when drawing each slime segment
private AnimatedSprite _sprite;