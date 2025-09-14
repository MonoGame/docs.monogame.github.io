protected override void LoadContent()
{
    // ...

    PointLightMaterial = SharedContent.WatchMaterial("effects/pointLightEffect");
    PointLightMaterial.IsDebugVisible = true;
    PointLightMaterial.SetParameter("LightBrightness", .25f);
    PointLightMaterial.SetParameter("LightSharpness", .1f);

    // ...
}