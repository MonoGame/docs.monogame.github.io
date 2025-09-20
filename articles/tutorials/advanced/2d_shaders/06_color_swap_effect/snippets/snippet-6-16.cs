public override void Draw(GameTime gameTime)
{
    // ...

    Core.SpriteBatch.Begin(
        samplerState: SamplerState.PointClamp,
        sortMode: SpriteSortMode.Immediate,
        effect: _colorSwapMaterial.Effect);

    // ...
}