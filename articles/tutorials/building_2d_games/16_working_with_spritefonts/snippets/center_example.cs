// The text to draw.
string message = "Hello, MonoGame!";

// Measure the size of the message to get the text dimensions.
Vector2 textSize = font.MeasureString(message);

// Set the origin to the center of the text dimensions.
Vector2 origin = textSize * 0.5f;

// Position will be the center of the screen.
Vector2 position = new Vector2(
    GraphicsDevice.PresentationParameters.BackBufferWidth,
    GraphicsDevice.PresentationParameters.BackBufferHeight
) * 0.5f;

// Draw centered text
_spriteBatch.DrawString(
    font,                   // font
    message,                // text
    position,               // position
    Color.White,            // color
    0.0f,                   // rotation
    origin,                 // origin
    1.0f,                   // scale
    SpriteEffects.None,     // effects
    0.0f                    // layerDepth
);
