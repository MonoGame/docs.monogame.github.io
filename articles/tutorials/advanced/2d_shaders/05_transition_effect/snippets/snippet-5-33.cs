protected override void Update(GameTime gameTime)
{
    // ...
    
    // Check if the scene transition material needs to be reloaded.
    SceneTransitionMaterial.SetParameter("Progress", SceneTransition.DirectionalRatio);
    SceneTransitionMaterial.Update();

    base.Update(gameTime);
}