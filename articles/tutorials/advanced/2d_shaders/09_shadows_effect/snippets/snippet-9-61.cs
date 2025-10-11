public void DrawLights(List<PointLight> lights, List<ShadowCaster> shadowCasters)
{
	// ...

    Core.SpriteBatch.End();

    Core.SpriteBatch.Begin(
        depthStencilState: _stencilTest,
        effect: Core.PointLightMaterial.Effect,
        blendState: BlendState.Additive
    );

    Core.SpriteBatch.Draw(NormalBuffer, rect, light.Color);
    Core.SpriteBatch.End();

	// ...

}