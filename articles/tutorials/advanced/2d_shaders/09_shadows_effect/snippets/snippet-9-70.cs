    protected override void Update(GameTime gameTime)
    {
        // ...
        
        DeferredCompositeMaterial.SetParameter("ScreenSize", new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
        DeferredCompositeMaterial.Update();
        
        base.Update(gameTime);
    }