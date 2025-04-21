public override void Update(GameTime gameTime)
{
    // Update the offsets for the background pattern wrapping so that it
    // scrolls down and to the right.
    float offset = _scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    _backgroundOffset.X -= offset;
    _backgroundOffset.Y -= offset;

    // Ensure that the offsets do not go beyond the texture bounds so it is
    // a seamless wrap
    _backgroundOffset.X %= _backgroundPattern.Width;
    _backgroundOffset.Y %= _backgroundPattern.Height;

    GumService.Default.Update(gameTime);
}