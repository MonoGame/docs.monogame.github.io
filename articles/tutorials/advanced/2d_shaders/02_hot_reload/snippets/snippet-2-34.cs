/// <summary>  
/// Attempts to refresh the asset if it has changed on disk using the registered owner <see cref="ContentManager"/>.  
/// </summary>
public bool TryRefresh(out T oldAsset)
{
    return Owner.TryRefresh(this, out oldAsset);
}
