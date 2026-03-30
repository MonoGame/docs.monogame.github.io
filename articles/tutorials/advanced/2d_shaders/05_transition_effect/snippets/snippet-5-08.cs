protected override void Draw(GameTime gameTime)
{
    // If there is an active scene, draw it.
    if (s_activeScene != null)
    {
        s_activeScene.Draw(gameTime);
    }
    
    // Draw the scene transition quad
    SpriteBatch.Begin(effect: SceneTransitionMaterial.Effect);  
    SpriteBatch.Draw(Pixel, GraphicsDevice.Viewport.Bounds, Color.White);  
    SpriteBatch.End();
    
    Material.DrawVisibleDebugUi(gameTime);

    base.Draw(gameTime);
}