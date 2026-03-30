public override void Draw(GameTime gameTime)
{
    // ...

    // render the shadow buffers  
    var casters = new List<ShadowCaster>();
    casters.AddRange(_shadowCasters);
    casters.AddRange(_slime.ShadowCasters);
    casters.Add(_bat.ShadowCaster);

    // Remove these
    // PointLight.DrawShadows(_lights, casters);
    // _deferredRenderer.StartLightPhase();
    // PointLight.Draw(Core.SpriteBatch, _lights, _deferredRenderer.NormalBuffer);

    // start rendering the lights  
    _deferredRenderer.DrawLights(_lights, casters);

    // finish the deferred rendering  
    _deferredRenderer.Finish();

    // ...
}