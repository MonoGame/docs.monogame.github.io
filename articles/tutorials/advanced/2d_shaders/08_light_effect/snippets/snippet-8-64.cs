public static void Draw(SpriteBatch spriteBatch, List<PointLight> pointLights, Texture2D normalBuffer)
{
	spriteBatch.Begin(
		effect: Core.PointLightMaterial.Effect,
		blendState: BlendState.Additive
		);
	
	foreach (var light in pointLights)
	{
		var diameter = light.Radius * 2;
		var rect = new Rectangle((int)(light.Position.X - light.Radius), (int)(light.Position.Y - light.Radius), diameter, diameter);
		spriteBatch.Draw(normalBuffer, rect, light.Color);
	}
	
	spriteBatch.End();
}
