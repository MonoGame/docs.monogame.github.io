private void CheckGamePadInput()
{
    // Get the gamepad info for gamepad one.
    GamePadInfo gamePadOne = Core.Input.GamePads[(int)PlayerIndex.One];

    // If the start button is pressed, pause the game
    if (gamePadOne.WasButtonJustPressed(Buttons.Start))
    {
        PauseGame();
    }

    // Existing gamepad input code
    // ...