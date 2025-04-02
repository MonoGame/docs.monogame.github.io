/// <summary>
/// Creates a new sprite using the region from this texture atlas with the specified name.
/// </summary>
/// <param name="regionName">The name of the region to create the sprite with.</param>
/// <returns>A new Sprite using the texture region with the specified name.</returns>
public Sprite CreateSprite(string regionName)
{
    TextureRegion region = GetRegion(regionName);
    return new Sprite(region);
}