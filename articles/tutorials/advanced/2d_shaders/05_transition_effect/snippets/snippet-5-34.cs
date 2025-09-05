// Draw the scene transition quad  
SpriteBatch.Begin(effect: SceneTransitionMaterial.Effect);  
SpriteBatch.Draw(SceneTransitionTextures[SceneTransition.TextureIndex % SceneTransitionTextures.Count], GraphicsDevice.Viewport.Bounds, Color.White);  
SpriteBatch.End();
