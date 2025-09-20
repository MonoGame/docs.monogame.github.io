public override void Draw(GameTime gameTime)
{
    // ...
    Core.SpriteBatch.End();

    // render the shadow buffers
    PointLight.DrawShadows(_lights, _shadowCasters);

    // start rendering the lights
    _deferredRenderer.StartLightPhase();

    // ...
}