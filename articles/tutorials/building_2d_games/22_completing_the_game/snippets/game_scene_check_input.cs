private void CheckInput()
{
    // Check for pause action
    if (_controller.Pause())
    {
        _pauseMenu.IsEnabled = _pauseMenu.IsVisible = _pauseMenu.IsSelected = true;
        return;
    }

    // Store the potential direction change
    Vector2 potentialNextDirection = _nextDirection;

    // Check movement actions
    if (_controller.MoveUp())
    {
        potentialNextDirection = -Vector2.UnitY;
    }
    else if (_controller.MoveDown())
    {
        potentialNextDirection = Vector2.UnitY;
    }
    else if (_controller.MoveLeft())
    {
        potentialNextDirection = -Vector2.UnitX;
    }
    else if (_controller.MoveRight())
    {
        potentialNextDirection = Vector2.UnitX;
    }

    // Only allow direction change if it is not reversing the current direction.
    // This prevents the "snake" from backing into itself
    if (Vector2.Dot(potentialNextDirection, _slimes[0].Direction) >= 0)
    {
        _nextDirection = potentialNextDirection;
    }
}
