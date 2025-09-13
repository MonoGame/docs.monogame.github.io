public override void LoadContent()
{
    // ...

    // Load the colorSwap material  
    _colorSwapMaterial = Core.SharedContent.WatchMaterial("effects/colorSwapEffect");
    _colorSwapMaterial.IsDebugVisible = true;

    // ...
}
