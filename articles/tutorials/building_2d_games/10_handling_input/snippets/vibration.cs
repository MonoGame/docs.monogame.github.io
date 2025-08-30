// Make the gamepad vibrate at full intensity.
GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);

// Stop all vibration.
GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);

// Create a subtle, low-intensity vibration.
GamePad.SetVibration(PlayerIndex.One, 0.3f, 0.1f);