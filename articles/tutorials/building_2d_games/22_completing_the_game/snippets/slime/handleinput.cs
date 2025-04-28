private void HandleInput()
{
    Vector2 potentialNextDirection = _nextDirection;

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

    // Only allow direction change if it is not reversing the current
    // direction.  This prevents the slime from backing into itself.
    float dot = Vector2.Dot(potentialNextDirection, _segments[0].Direction);
    if (dot >= 0)
    {
        _nextDirection = potentialNextDirection;
    }
}