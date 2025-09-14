public DeferredRenderer()
{
    // ...

    NormalBuffer = new RenderTarget2D(
        graphicsDevice: Core.GraphicsDevice,
        width: viewport.Width,
        height: viewport.Height,
        mipMap: false,
        preferredFormat: SurfaceFormat.Color,
        preferredDepthFormat: DepthFormat.None);

    // ...
}