public void DrawLights(List<PointLight> lights, List<ShadowCaster> shadowCasters)
{
    // ...

    Core.ShadowHullMaterial.SetParameter("LightPosition", light.Position);
    Core.SpriteBatch.Begin(
        depthStencilState: _stencilWrite,
        effect: Core.ShadowHullMaterial.Effect,
        blendState: _shadowBlendState,
        rasterizerState: RasterizerState.CullNone
    );

    foreach (var caster in shadowCasters)

    // ...

}