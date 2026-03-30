protected override void LoadContent()
{
    base.LoadContent();

    // ...

    PointLightMaterial = SharedContent.WatchMaterial("effects/pointLightEffect");
    PointLightMaterial.IsDebugVisible = true;
}