public override void Draw(GameTime gameTime)
{
    // ...

    // Always end the sprite batch when finished.
    Core.SpriteBatch.End();

    // render the shadow buffers  
    var casters = new List<ShadowCaster>();
    casters.AddRange(_shadowCasters);
    casters.AddRange(_slime.ShadowCasters);
    PointLight.DrawShadows(_lights, casters);

    // start rendering the lights  
    _deferredRenderer.StartLightPhase();
    PointLight.Draw(Core.SpriteBatch, _lights, _deferredRenderer.NormalBuffer);

    // ...
}