public void DrawLights(List<PointLight> lights, List<ShadowCaster> shadowCasters)
{
    Core.GraphicsDevice.SetRenderTarget(LightBuffer);
    Core.GraphicsDevice.Clear(Color.Black);
    
    foreach (var light in lights)
    {
        Core.SpriteBatch.Begin(
            effect: Core.PointLightMaterial.Effect,
            blendState: BlendState.Additive
        );

        var diameter = light.Radius * 2;
        var rect = new Rectangle(
            (int)(light.Position.X - light.Radius), 
            (int)(light.Position.Y - light.Radius),
            diameter, diameter);
        Core.SpriteBatch.Draw(NormalBuffer, rect, light.Color);
        Core.SpriteBatch.End();

    }
}
