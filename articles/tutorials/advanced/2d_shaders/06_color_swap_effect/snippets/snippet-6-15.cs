public override void LoadContent()
{
    // ...

    _slimeColorMap = new RedColorMap();  
    _slimeColorMap.SetColorsByRedValue(new Dictionary<int, Color>  
    {  
        // main color  
        [32] = Color.Khaki,  
        // wall color  
        [115] = Color.Coral,  
        // shadow color  
        [214] = Color.MonoGameOrange,  
        // floor  
        [255] = Color.Tomato  
    });  
    
    _colorSwapMaterial.SetParameter("ColorMap", _slimeColorMap.ColorMap);

}
