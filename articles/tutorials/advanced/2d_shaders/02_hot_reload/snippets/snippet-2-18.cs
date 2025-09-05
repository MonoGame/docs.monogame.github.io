public static T Watch<T>(this ContentManager manager, string assetName)  
{  
    var asset = manager.Load<T>(assetName);
    return asset;
}
