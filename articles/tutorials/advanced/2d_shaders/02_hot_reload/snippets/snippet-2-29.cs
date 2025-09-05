public static bool TryRefresh<T>(this ContentManager manager, WatchedAsset<T> watchedAsset, out T oldAsset)
{
    oldAsset = default;
    
    var path = Path.Combine(manager.RootDirectory, watchedAsset.AssetName) + ".xnb";
    var lastWriteTime = File.GetLastWriteTime(path);
    
    if (lastWriteTime <= watchedAsset.UpdatedAt)
    {
        return false;
    }

    if (IsFileLocked(path)) return false; // wait for the file to not be locked.
    
    manager.UnloadAsset(watchedAsset.AssetName);
    oldAsset = watchedAsset.Asset;
    watchedAsset.Asset = manager.Load<T>(watchedAsset.AssetName);
    watchedAsset.UpdatedAt = lastWriteTime;
    
    return true;
}