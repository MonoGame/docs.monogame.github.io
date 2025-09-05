/// <summary>  
/// Load an Effect into the <see cref="Material"/> wrapper class  
/// </summary>  
/// <param name="manager"></param>  
/// <param name="assetName"></param>  
/// <returns></returns>  
public static Material WatchMaterial(this ContentManager manager, string assetName)  
{  
    return new Material(manager.Watch<Effect>(assetName));  
}
