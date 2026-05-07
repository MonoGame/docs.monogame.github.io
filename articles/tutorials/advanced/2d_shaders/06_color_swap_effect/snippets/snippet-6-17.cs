public override void Draw(GameTime gameTime)
{
    // ...

    // Update the colorMap
    _colorSwapMaterial.SetParameter("ColorMap", _colorMap);

    // Draw the tilemap
    _tilemap.Draw(Core.SpriteBatch);

    // Draw the bat.
    _bat.Draw();

    // Update the colorMap for the slime  
    _colorSwapMaterial.SetParameter("ColorMap", _slimeColorMap.ColorMap);

    // Draw the slime.
    _slime.Draw();

    // Always end the sprite batch when finished.
    Core.SpriteBatch.End();

    // Draw the UI
    _ui.Draw();
}