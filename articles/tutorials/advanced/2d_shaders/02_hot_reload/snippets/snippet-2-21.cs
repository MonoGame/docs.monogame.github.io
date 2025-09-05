public static WatchedAsset<T> Watch<T>(this ContentManager manager, string assetName)
{
    var asset = manager.Load<T>(assetName);
    return new WatchedAsset<T>
    {
        AssetName = assetName,
        Asset = asset,
        UpdatedAt = DateTimeOffset.Now,
    };
}
