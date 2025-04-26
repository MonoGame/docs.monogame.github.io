    private void CheckKeyboardInput()
    {
        // Get a reference to the keyboard info
        KeyboardInfo keyboard = Core.Input.Keyboard;

        // If the escape key is pressed, pause the game.
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape))
        {
            PauseGame();
        }

        // Existing keyboard input code
        // ...