public void DrawLights(List<PointLight> lights, List<ShadowCaster> shadowCasters, Action<BlendState, DepthStencilState> prepareStencil)
{
    Core.GraphicsDevice.SetRenderTarget(LightBuffer);
    Core.GraphicsDevice.Clear(Color.Black);
    foreach (var light in lights)
    {
        var diameter = light.Radius * 2;
        var rect = new Rectangle(
            (int)(light.Position.X - light.Radius),
            (int)(light.Position.Y - light.Radius),
            diameter, diameter);

        // initialize the stencil to '1'.
        Core.GraphicsDevice.Clear(ClearOptions.Stencil, Color.Black, 0, 1);

        // Anything that draws in this setup will set the stencil back to '0'. This '0' acts as a "don't draw a shadow here". 
        prepareStencil?.Invoke(_shadowBlendState, _stencilShadowExclude);

        // ...

    }

    // ...
}