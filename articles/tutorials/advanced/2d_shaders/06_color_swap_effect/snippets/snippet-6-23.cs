private void CollisionChecks(GameTime gameTime)
{
    // ...
    if (slimeBounds.Intersects(batBounds))
    {
        // ...

        // Tell the slime to grow.
        _slime.Grow();

        // Remember when the last time the slime grew  
        _lastGrowTime = gameTime.TotalGameTime;

        // ...
    }

    // ...
}