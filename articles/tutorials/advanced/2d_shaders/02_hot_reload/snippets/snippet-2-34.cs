public bool TryRefresh(out T oldAsset)  
{  
    return Owner.TryRefresh(this, out oldAsset);  
}
