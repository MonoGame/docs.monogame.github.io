protected override void LoadContent()
{
    base.LoadContent();
    SceneTransitionMaterial = Content.WatchMaterial("effects/sceneTransitionEffect.fx");  
    SceneTransitionMaterial.IsDebugVisible = true;
}

