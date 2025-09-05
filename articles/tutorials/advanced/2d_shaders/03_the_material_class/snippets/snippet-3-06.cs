	// We are in a game over state, so apply the saturation parameter.  
	_grayscaleEffect.Effect.Parameters["Saturation"].SetValue(_saturation);  
	  
	// And begin the sprite batch using the grayscale effect.  
	Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, effect: _grayscaleEffect.Effect);
