public override void Update(GameTime gameTime)  
{  
    // Ensure the UI is always updated  
    _ui.Update(gameTime);  
  
    // Update the grayscale effect if it was changed  
    _grayscaleEffect.Update();  
    _colorSwapMaterial.Update();  

    // Prevent the game from actually updating. TODO: remove this when we are done playing with shaders!
    return;

    // ...
