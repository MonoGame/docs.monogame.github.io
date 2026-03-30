public override void Draw(GameTime gameTime)
{
    // Clear the back buffer.
    Core.GraphicsDevice.Clear(Color.CornflowerBlue);
        
    _colorSwapMaterial.SetParameter("Saturation", _saturation);
    
    Core.SpriteBatch.Begin(
        samplerState: SamplerState.PointClamp,
        sortMode: SpriteSortMode.Immediate,
        effect: _colorSwapMaterial.Effect);

    // Update the colorMap
    _colorSwapMaterial.SetParameter("ColorMap", _colorMap);

    // ...
}