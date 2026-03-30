protected override void LoadContent()
{
    base.LoadContent();
    SceneTransitionMaterial = Content.WatchMaterial("effects/sceneTransitionEffect");  
    SceneTransitionMaterial.IsDebugVisible = true;
}

