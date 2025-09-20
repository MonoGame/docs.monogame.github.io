protected override void LoadContent()
{
    base.LoadContent();
    SceneTransitionMaterial = SharedContent.WatchMaterial("effects/sceneTransitionEffect");
    SceneTransitionMaterial.SetParameter("EdgeWidth", .05f);

    SceneTransitionTextures = new List<Texture2D>();
    SceneTransitionTextures.Add(SharedContent.Load<Texture2D>("images/angled"));
    SceneTransitionTextures.Add(SharedContent.Load<Texture2D>("images/concave"));
    SceneTransitionTextures.Add(SharedContent.Load<Texture2D>("images/radial"));
    SceneTransitionTextures.Add(SharedContent.Load<Texture2D>("images/ripple"));
}
