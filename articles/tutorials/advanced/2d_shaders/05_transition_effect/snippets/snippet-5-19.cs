protected override void LoadContent()
{
    base.LoadContent();
    SceneTransitionMaterial = SharedContent.WatchMaterial("effects/sceneTransitionEffect");
    SceneTransitionMaterial.SetParameter("EdgeWidth", .05f);

    // ...
}