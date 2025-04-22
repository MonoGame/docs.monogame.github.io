/// <summary>
/// Gets the top normalized texture coordinate of this region.
/// </summary>
public float TopTextureCoordinate => SourceRectangle.Top / (float)Texture.Height;

/// <summary>
/// Gets the bottom normalized texture coordinate of this region.
/// </summary>
public float BottomTextureCoordinate => SourceRectangle.Bottom / (float)Texture.Height;

/// <summary>
///  Gets the left normalized texture coordinate of this region.
/// </summary>
public float LeftTextureCoordinate => SourceRectangle.Left / (float)Texture.Width;

/// <summary>
/// Gets the right normalized texture coordinate of this region.
/// </summary>
public float RightTextureCoordinate => SourceRectangle.Right / (float)Texture.Width;