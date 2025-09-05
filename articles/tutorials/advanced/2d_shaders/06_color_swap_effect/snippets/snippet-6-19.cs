_slimeColorMap = new RedColorMap();
_slimeColorMap.SetColorsByExistingColorMap(_colorMap);
_slimeColorMap.SetColorsByRedValue(new Dictionary<int, Color>
{
    // main color
    [32] = Color.Yellow,
}, false);
