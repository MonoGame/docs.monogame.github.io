if (_state != GameState.Playing)  
{  
    // We are in a game over state, so apply the saturation parameter.  
    _grayscaleEffect.SetParameter("Saturation", _saturation);  
  
    // And begin the sprite batch using the grayscale effect.  
    Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, effect: _colorSwapMaterial.Effect);  
}  
else  
{  
    // Otherwise, just begin the sprite batch as normal.  
    Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, effect: _colorSwapMaterial.Effect);  
}
