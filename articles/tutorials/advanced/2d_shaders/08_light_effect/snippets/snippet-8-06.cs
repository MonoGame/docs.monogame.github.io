    public override void Draw(GameTime gameTime)
    {
        // ... configure the sprite batch 
        
        // Start rendering to the deferred renderer
        _deferredRenderer.StartColorPhase();
        Core.SpriteBatch.Begin(
            samplerState: SamplerState.PointClamp,
            sortMode: SpriteSortMode.Immediate,
            rasterizerState: RasterizerState.CullNone,
            effect: _gameMaterial.Effect);

		// ... all of the actual draw code.
		
        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();
        _deferredRenderer.Finish();

        // Draw the UI
        _ui.Draw();
    }
