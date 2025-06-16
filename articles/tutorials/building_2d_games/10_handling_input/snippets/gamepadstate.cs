// Get the current state of player one's gamepad.
GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

// Check if the A button is pressed down.
if(gamePadState.Buttons.A == ButtonState.Pressed)
{
    // Button A is pressed, do something.
}