protected override void Update(GameTime gameTime)
{
    // ...        
    
    // if there is a next scene waiting to be switch to, then transition
    // to that scene
    if (s_nextScene != null && SceneTransition.IsComplete)
    {
        TransitionScene();
    }

    // ...
}