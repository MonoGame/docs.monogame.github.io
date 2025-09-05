protected override void Draw(GameTime gameTime)
{
	// If there is an active scene, draw it.
	if (s_activeScene != null)
	{
		s_activeScene.Draw(gameTime);
	}

	Material.DrawVisibleDebugUi(gameTime);
	
	base.Draw(gameTime);
}
