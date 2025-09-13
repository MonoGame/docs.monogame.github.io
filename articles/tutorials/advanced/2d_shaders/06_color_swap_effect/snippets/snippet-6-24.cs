public override void Draw(GameTime gameTime)
{
    // ...

    // Draw the slime.
    _slime.Draw(segmentIndex =>
    {
        const int flashTimeMs = 125;
        var map = _colorMap;
        var elapsedMs = (gameTime.TotalGameTime.TotalMilliseconds - _lastGrowTime.TotalMilliseconds);
        var intervalsAgo = (int)(elapsedMs / flashTimeMs);

        if (intervalsAgo < _slime.Size && (intervalsAgo - segmentIndex) % _slime.Size == 0)
        {
            map = _slimeColorMap.ColorMap;
        }

        _colorSwapMaterial.SetParameter("ColorMap", map);
    });

    // ...
}