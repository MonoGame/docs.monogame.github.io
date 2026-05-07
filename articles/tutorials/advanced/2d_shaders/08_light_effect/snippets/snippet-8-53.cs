public static void Draw(SpriteBatch spriteBatch, List<PointLight> pointLights, Texture2D normalBuffer)
{
    Core.PointLightMaterial.SetParameter("NormalBuffer", normalBuffer);
    // ...

}