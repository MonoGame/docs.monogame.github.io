// Get the current state of player one's gamepad.
GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

// Check if the A button is down.
if(gamePadState.IsButtonDown(Buttons.A))
{
    // The A button is pressed, do something.
}