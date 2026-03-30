public void DrawShadowBuffer(List<ShadowCaster> shadowCasters)
{
    /// ...
    
    foreach (var caster in shadowCasters)
    {
        var posA = caster.A;
        var aToB = (caster.B - caster.A) / screenSize;
        var packed = PackVector2_SNorm(aToB);
        Core.SpriteBatch.Draw(Core.Pixel, posA, packed);
    }
    
    Core.SpriteBatch.End();
}
