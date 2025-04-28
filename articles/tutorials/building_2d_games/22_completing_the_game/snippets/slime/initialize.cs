/// <summary>
/// Initializes the slime, can be used to reset it back to an initial state.
/// </summary>
/// <param name="startingPosition">The position the slime should start at.</param>
/// <param name="stride">The total number of pixels to move the head segment during each movement cycle.</param>
public void Initialize(Vector2 startingPosition, float stride)
{
    // Initialize the segment collection.
    _segments = new List<SlimeSegment>();

    // Set the stride
    _stride = stride;

    // Create the initial head of the slime chain.
    SlimeSegment head = new SlimeSegment();
    head.At = startingPosition;
    head.To = startingPosition + new Vector2(_stride, 0);
    head.Direction = Vector2.UnitX;

    // Add it to the segment collection.
    _segments.Add(head);

    // Set the initial next direction as the same direction the head is
    // moving.
    _nextDirection = head.Direction;

    // Zero out the movement timer.
    _movementTimer = TimeSpan.Zero;
}