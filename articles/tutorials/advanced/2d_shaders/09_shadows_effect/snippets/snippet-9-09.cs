protected override void LoadContent()
{
    base.LoadContent();

    // ...

    ShadowHullMaterial = SharedContent.WatchMaterial("effects/shadowHullEffect");
    ShadowHullMaterial.IsDebugVisible = true;

    // ...
}
