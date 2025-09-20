public override void Draw(GameTime gameTime)
{
    // ...

    // Update the colorMap for the slime
    if ((int)gameTime.TotalGameTime.TotalSeconds % 2 == 0)
    {
        _colorSwapMaterial.SetParameter("ColorMap", _slimeColorMap.ColorMap);
    }

    // Draw the slime.
    _slime.Draw();

    // ...
}