protected override void LoadContent()
{
    // ...

    DeferredCompositeMaterial = SharedContent.WatchMaterial("effects/deferredCompositeEffect");
    DeferredCompositeMaterial.IsDebugVisible = true;
}