/// <summary>
/// Draws the slime.
/// </summary>
public void Draw()
{
    // Iterate through each segment and draw it
    foreach (SlimeSegment segment in _segments)
    {
        // Calculate the visual position of the segment at the moment by
        // lerping between its "at" and "to" position by the movement
        // offset lerp amount
        Vector2 pos = Vector2.Lerp(segment.At, segment.To, _movementProgress);

        // Draw the slime sprite at the calculated visual position of this
        // segment
        _sprite.Draw(Core.SpriteBatch, pos);
    }
}