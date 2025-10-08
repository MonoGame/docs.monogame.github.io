protected override void LoadContent()
{
    base.LoadContent();
    SceneTransitionMaterial = Content.WatchMaterial("effects/sceneTransitionEffect");
    SceneTransitionMaterial.SetParameter("EdgeWidth", .05f);
    SceneTransitionMaterial.IsDebugVisible = true;

    SceneTransitionTextures = new List<Texture2D>();
    SceneTransitionTextures.Add(Content.Load<Texture2D>("images/angled"));
    SceneTransitionTextures.Add(Content.Load<Texture2D>("images/concave"));
    SceneTransitionTextures.Add(Content.Load<Texture2D>("images/radial"));
    SceneTransitionTextures.Add(Content.Load<Texture2D>("images/ripple"));
}