// Track the state of keyboard input during the previous frame.
private KeyboardState _previousKeyboardState;

protected override void Update(GameTime gameTime)
{
    // Get the current state of keyboard input.
    KeyboardState keyboardState = Keyboard.GetState();

    // Compare if the space key is down on the current frame but was up on the previous frame.
    if (keyboardState.IsKeyDown(Keys.Space) && _previousKeyboardState.IsKeyUp(Keys.Space))
    {
        // This will only run on the first frame Space is pressed and will not
        // happen again until it has been released and then pressed again.
    }

    // At the end of update, store the current state of keyboard input into the
    // previous state tracker.
    _previousKeyboardState = keyboardState;
    
    base.Update(gameTime);
}