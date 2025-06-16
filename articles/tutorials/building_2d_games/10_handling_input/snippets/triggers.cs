// Get the current state of player one's gamepad.
GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

// Get the acceleration based on how far the right trigger is pushed down.
float acceleration = gamePadState.Triggers.Right;