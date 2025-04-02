// Get the current state of keyboard input.
KeyboardState keyboardState = Keyboard.GetState();

// Check if the space key is down.
if (keyboardState.IsKeyDown(Keys.Space))
{
    // This runs EVERY frame the space key is held down
}