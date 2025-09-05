[Conditional("DEBUG")]
public static void DrawVisibleDebugUi(GameTime gameTime)
{
	// first, cull any materials that are not visible, or disposed. 
	var toRemove = new List<Material>();
	foreach (var material in s_debugMaterials)
	{
		if (material.Effect.IsDisposed)
		{
			toRemove.Add(material);
		}
	}

	foreach (var material in toRemove)
	{
		s_debugMaterials.Remove(material);
	}
	
	Core.ImGuiRenderer.BeforeLayout(gameTime);
	foreach (var material in s_debugMaterials)
	{
		material.DrawDebug();
	}
	Core.ImGuiRenderer.AfterLayout();
}
