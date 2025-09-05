public static void DrawShadows(
	List<PointLight> pointLights,
	List<ShadowCaster> shadowCasters)
{
	foreach (var light in pointLights)
	{
		light.DrawShadowBuffer(shadowCasters);
	}
}
