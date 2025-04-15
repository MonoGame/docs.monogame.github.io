// Tracks the segments of the slime chain.
private List<SlimeSegment> _slimes;

// The next direction to apply to the head of the slime chain
// during the next movement update.
private Vector2 _nextDirection;

// The amount of time that has elapsed since the last movement update.
private TimeSpan _tickTimer;

// A constant value that provides the amount of time to wait between
// movement updates.
private readonly static TimeSpan s_tickTime = TimeSpan.FromMilliseconds(500);

// The control input profile for the game.
private GameController _controller;
