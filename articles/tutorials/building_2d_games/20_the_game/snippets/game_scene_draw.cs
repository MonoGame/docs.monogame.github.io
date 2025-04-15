public override void Draw(GameTime gameTime)
{
    // Prior existing code for this method...

    // Draw the tilemap
    _tilemap.Draw(Core.SpriteBatch);

    // Draw each slime
    float lerpAmount = (float)(_tickTimer.TotalSeconds / s_tickTime.TotalSeconds);
    foreach (SlimeSegment segment in _slimes)
    {
        Vector2 pos = Vector2.Lerp(segment.At, segment.To, lerpAmount);
        _slime.Draw(Core.SpriteBatch, pos);
    }

    // Draw the bat sprite.
    _bat.Draw(Core.SpriteBatch, _batPosition);

    // Rest of existing code for this method...
}
