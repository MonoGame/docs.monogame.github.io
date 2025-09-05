_colorSwapMaterial.SetParameter("Saturation", _saturation);
Core.SpriteBatch.Begin(
    samplerState: SamplerState.PointClamp,
    sortMode: SpriteSortMode.Immediate,
    effect: _colorSwapMaterial.Effect);
