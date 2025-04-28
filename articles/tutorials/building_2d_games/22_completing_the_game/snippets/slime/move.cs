private void Move()
{
    // Capture the value of the head segment
    SlimeSegment head = _segments[0];

    // Update the direction the head is supposed to move in to the
    // next direction cached.
    head.Direction = _nextDirection;

    // Update the head's "at" position to be where it was moving "to"
    head.At = head.To;

    // Update the head's "to" position to the next tile in the direction
    // it is moving.
    head.To = head.At + head.Direction * _stride;

    // Insert the new adjusted value for the head at the front of the
    // segments and remove the tail segment. This effectively moves
    // the entire chain forward without needing to loop through every
    // segment and update its "at" and "to" positions.
    _segments.Insert(0, head);
    _segments.RemoveAt(_segments.Count - 1);

    // Iterate through all of the segments except the head and check
    // if they are at the same position as the head. If they are, then
    // the head is colliding with a body segment and a body collision
    // has occurred.
    for (int i = 1; i < _segments.Count; i++)
    {
        SlimeSegment segment = _segments[i];

        if (head.At == segment.At)
        {
            if(BodyCollision != null)
            {
                BodyCollision.Invoke(this, EventArgs.Empty);
            }

            return;
        }
    }
}