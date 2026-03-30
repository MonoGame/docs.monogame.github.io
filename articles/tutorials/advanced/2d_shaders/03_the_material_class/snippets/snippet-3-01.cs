using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Content;

namespace MonoGameLibrary.Graphics;

public class Material
{
    /// <summary>
    /// The hot-reloadable asset that this material is using
    /// </summary>
    public WatchedAsset<Effect> Asset;
    
    /// <summary>
    /// The currently loaded Effect that this material is using
    /// </summary>
    public Effect Effect => Asset.Asset;

    public Material(WatchedAsset<Effect> asset)
    {
        Asset = asset;
    }
}
