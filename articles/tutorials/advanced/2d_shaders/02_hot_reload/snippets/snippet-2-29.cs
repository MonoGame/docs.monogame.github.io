public static bool TryRefresh<T>(this ContentManager manager, WatchedAsset<T> watchedAsset, out T oldAsset)
{
    oldAsset = default;

    // get the same path that the ContentManager would use to load the asset
    var path = Path.Combine(manager.RootDirectory, watchedAsset.AssetName) + ".xnb";

    // ask the operating system when the file was last written.
    var lastWriteTime = File.GetLastWriteTime(path);

    //  when the file's write time is less recent than the asset's latest read time, 
    //  then the asset does not need to be reloaded.
    if (lastWriteTime <= watchedAsset.UpdatedAt)
    {
        return false;
    }

    // wait for the file to not be locked.
    if (IsFileLocked(path)) return false;

    // clear the old asset to avoid leaking
    manager.UnloadAsset(watchedAsset.AssetName);

    // return the old asset
    oldAsset = watchedAsset.Asset;

    // load the new asset and update the latest read time
    watchedAsset.Asset = manager.Load<T>(watchedAsset.AssetName);
    watchedAsset.UpdatedAt = lastWriteTime;

    return true;
}