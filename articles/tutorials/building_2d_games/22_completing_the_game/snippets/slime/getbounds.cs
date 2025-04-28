/// <summary>
/// Returns a Circle value that represents collision bounds of the slime.
/// </summary>
/// <returns>A Circle value.</returns>
public Circle GetBounds()
{
    SlimeSegment head = _segments[0];

    // Calculate the visual position of the head at the moment of this
    // method call by lerping between the "at" and "to" position by the
    // movement offset lerp amount
    Vector2 pos = Vector2.Lerp(head.At, head.To, _movementProgress);

    // Create the bounds using the calculated visual position of the head.
    Circle bounds = new Circle(
        (int)(pos.X + (_sprite.Width * 0.5f)),
        (int)(pos.Y + (_sprite.Height * 0.5f)),
        (int)(_sprite.Width * 0.5f)
    );

    return bounds;
}