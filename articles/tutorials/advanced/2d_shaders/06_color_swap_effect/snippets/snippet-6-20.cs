public override void Draw(GameTime gameTime)
{
    // ...

    // Draw the bat.
    _bat.Draw();
    
    // Update the colorMap for the slime
    if ((int)gameTime.TotalGameTime.TotalSeconds % 2 == 0)
    {
        _colorSwapMaterial.SetParameter("ColorMap", _slimeColorMap.ColorMap);
    }

    // Draw the slime.
    _slime.Draw();

    // ...
}