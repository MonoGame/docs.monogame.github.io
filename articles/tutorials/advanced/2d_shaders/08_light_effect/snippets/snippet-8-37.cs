public void DrawComposite()
{
    Core.DeferredCompositeMaterial.SetParameter("LightBuffer", LightBuffer);
    var viewportBounds = Core.GraphicsDevice.Viewport.Bounds;
    Core.SpriteBatch.Begin(
        effect: Core.DeferredCompositeMaterial.Effect
        );
    Core.SpriteBatch.Draw(ColorBuffer, viewportBounds, Color.White);
    Core.SpriteBatch.End();
}