/// <summary>
/// Returns a Circle value that represents collision bounds of the bat.
/// </summary>
/// <returns>A Circle value.</returns>
public Circle GetBounds()
{
    int x = (int)(Position.X + _sprite.Width * 0.5f);
    int y = (int)(Position.Y + _sprite.Height * 0.5f);
    int radius = (int)(_sprite.Width * 0.25f);

    return new Circle(x, y, radius);
}