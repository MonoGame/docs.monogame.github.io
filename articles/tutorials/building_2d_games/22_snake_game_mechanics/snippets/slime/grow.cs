/// <summary>
/// Informs the slime to grow by one segment.
/// </summary>
public void Grow()
{
    // Capture the value of the tail segment
    SlimeSegment tail = _segments[_segments.Count - 1];

    // Create a new tail segment that is positioned a grid cell in the
    // reverse direction from the tail moving to the tail.
    SlimeSegment newTail = new SlimeSegment();
    newTail.At = tail.To + tail.ReverseDirection * _stride;
    newTail.To = tail.At;
    newTail.Direction = Vector2.Normalize(tail.At - newTail.At);

    // Add the new tail segment
    _segments.Add(newTail);
}