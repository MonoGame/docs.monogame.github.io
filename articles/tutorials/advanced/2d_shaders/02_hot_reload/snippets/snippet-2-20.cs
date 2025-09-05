using System;
using Microsoft.Xna.Framework.Content;

namespace MonoGameLibrary.Content;

public class WatchedAsset<T>
{
    /// <summary>
    /// The latest version of the asset.
    /// </summary>
    public T Asset { get; set; }
    
    /// <summary>
    /// The last time the <see cref="Asset"/> was loaded into memory.
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }
    
    /// <summary>
    /// The name of the <see cref="Asset"/>. This is the name used to load the asset from disk. 
    /// </summary>
    public string AssetName { get; init; }
}
