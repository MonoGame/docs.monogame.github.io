public void StartColorPhase()  
{  
    // all future draw calls will be drawn to the color buffer  
    Core.GraphicsDevice.SetRenderTarget(ColorBuffer);  
    Core.GraphicsDevice.Clear(Color.Transparent);  
}
