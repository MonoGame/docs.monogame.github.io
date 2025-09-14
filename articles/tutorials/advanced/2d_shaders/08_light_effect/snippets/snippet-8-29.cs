public static void Draw(SpriteBatch spriteBatch, List<PointLight> pointLights, Texture2D normalBuffer)
{
    spriteBatch.Begin(
        effect: Core.PointLightMaterial.Effect,
        blendState: BlendState.Additive
        );

    // ...
}