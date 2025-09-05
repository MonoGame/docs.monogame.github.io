public void SetColorsByExistingColorMap(Texture2D existingColorMap)
{
    var existingPixels = new Color[256];
    existingColorMap.GetData(existingPixels);

    var map = new Dictionary<int, Color>();
    for (var i = 0; i < existingPixels.Length; i++)
    {
        map[i] = existingPixels[i];
    }
    
    SetColorsByRedValue(map);
}
