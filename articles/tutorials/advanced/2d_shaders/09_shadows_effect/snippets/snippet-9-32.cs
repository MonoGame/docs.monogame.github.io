public void DrawShadowBuffer(List<ShadowCaster> shadowCasters)  
{  
    Core.GraphicsDevice.SetRenderTarget(ShadowBuffer);  
    // clear the shadow buffer to white to start  
    Core.GraphicsDevice.Clear(Color.White);
    // ...
