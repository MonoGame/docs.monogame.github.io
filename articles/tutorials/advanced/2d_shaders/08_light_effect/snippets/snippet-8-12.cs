public override void Draw(GameTime gameTime)
{
    // ...

    // Always end the sprite batch when finished.  
    Core.SpriteBatch.End();

    // start rendering the lights  
    _deferredRenderer.StartLightPhase();

    // TODO: draw lights  

    // finish the deferred rendering  
    _deferredRenderer.Finish();

    // ...
}