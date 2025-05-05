private void HandleInput()
{
    Vector2 potentialNextDirection = Vector2.Zero;

    if (GameController.MoveUp())
    {
        potentialNextDirection = -Vector2.UnitY;
    }
    else if (GameController.MoveDown())
    {
        potentialNextDirection = Vector2.UnitY;
    }
    else if (GameController.MoveLeft())
    {
        potentialNextDirection = -Vector2.UnitX;
    }
    else if (GameController.MoveRight())
    {
        potentialNextDirection = Vector2.UnitX;
    }

    // If a new direction was input, consider adding it to the buffer
    if (potentialNextDirection != Vector2.Zero && _inputBuffer.Count < MAX_BUFFER_SIZE)
    {
        // If the buffer is empty, validate against the current direction;
        // otherwise, validate against the last buffered direction
        Vector2 validateAgainst = _inputBuffer.Count > 0 ?
                                  _inputBuffer.Last() :
                                  _segments[0].Direction;

        // Only allow direction change if it is not reversing the current
        // direction.  This prevents th slime from backing into itself
        float dot = Vector2.Dot(potentialNextDirection, validateAgainst);
        if (dot >= 0)
        {
            _inputBuffer.Enqueue(potentialNextDirection);
        }
    }
}