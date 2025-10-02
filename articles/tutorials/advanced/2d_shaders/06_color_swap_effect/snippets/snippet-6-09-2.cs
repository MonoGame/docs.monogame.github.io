public override void LoadContent()
{
    // ...
    
    // Load the colorSwap material
    _colorSwapMaterial = Core.SharedContent.WatchMaterial("effects/colorSwapEffect");
    _colorSwapMaterial.IsDebugVisible = true;

    var colorMap = Content.Load<Texture2D>("images/color-map-1");
    _colorSwapMaterial.SetParameter("ColorMap", colorMap);
}
