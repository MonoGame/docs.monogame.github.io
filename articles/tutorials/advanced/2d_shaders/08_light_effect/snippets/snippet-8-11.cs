public void StartLightPhase()  
{  
    // all future draw calls will be drawn to the light buffer  
    Core.GraphicsDevice.SetRenderTarget(LightBuffer);  
    Core.GraphicsDevice.Clear(Color.Black);  
}
