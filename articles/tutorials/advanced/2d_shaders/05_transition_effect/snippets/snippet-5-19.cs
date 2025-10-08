protected override void LoadContent()
{
    base.LoadContent();
    SceneTransitionMaterial = Content.WatchMaterial("effects/sceneTransitionEffect");
    SceneTransitionMaterial.SetParameter("EdgeWidth", .05f);
    SceneTransitionMaterial.IsDebugVisible = true;
}