/// <summary>
/// Creates a new texture atlas.
/// </summary>
public TextureAtlas()
{
    _regions = new Dictionary<string, TextureRegion>();
    _animations = new Dictionary<string, Animation>();
}

/// <summary>
/// Creates a new texture atlas instance using the given texture.
/// </summary>
/// <param name="texture">The source texture represented by the texture atlas.</param>
public TextureAtlas(Texture2D texture)
{
    Texture = texture;
    _regions = new Dictionary<string, TextureRegion>();
    _animations = new Dictionary<string, Animation>();
}