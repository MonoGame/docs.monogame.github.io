/// <summary>
/// Draws the slime.
/// </summary>
public void Draw(Action<int> configureSpriteBatch)
{
    // Iterate through each segment and draw it
    for (var i = 0 ; i < _segments.Count; i ++)
    {
        var segment = _segments[i];
        // Calculate the visual position of the segment at the moment by
        // lerping between its "at" and "to" position by the movement
        // offset lerp amount
        Vector2 pos = Vector2.Lerp(segment.At, segment.To, _movementProgress);

        // Allow the sprite batch to be configured before each call.
        configureSpriteBatch(i);

        // Draw the slime sprite at the calculated visual position of this
        // segment
        _sprite.Draw(Core.SpriteBatch, pos);
    }
}
