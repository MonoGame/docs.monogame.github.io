// Get the current state of player one's gamepad.
GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

// Check if the down on the DPad is pressed.
if(gamePadState.DPad.Down == ButtonState.Pressed)
{
    // DPad down is pressed, do something.
}