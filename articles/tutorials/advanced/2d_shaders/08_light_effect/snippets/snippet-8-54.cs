public override void Draw(GameTime gameTime)
{
    // ...

    // start rendering the lights  
    _deferredRenderer.StartLightPhase();
    PointLight.Draw(Core.SpriteBatch, _lights, _deferredRenderer.NormalBuffer);

    // ...
}