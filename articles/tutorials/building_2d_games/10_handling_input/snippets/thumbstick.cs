// Get the current state of player one's gamepad.
GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

// Get the value of the left thumbstick.
Vector2 leftStick = gamePadState.Thumbsticks.Left;

// Invert the y-axis value.
leftStick.Y *= -1.0f;

// Apply the value to the position of the sprite.
sprite.Position += leftStick;