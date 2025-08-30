// Store the current location
Vector2 previousLocation = _spriteLocation;

// Calculate a new location
Vector2 newLocation = _spriteLocation + new Vector2(10, 0);

// Create a bounding box for the sprite object
Rectangle spriteBounds = new Rectangle(
    (int)newLocation.X,
    (int)newLocation.Y,
    (int)_sprite.Width,
    (int)_sprite.Height
);

// Get the bounds of the screen as a rectangle
Rectangle screenBounds = new Rectangle(
    0,
    0,
    GraphicsDevice.PresentationParameters.BackBufferWidth,
    GraphicsDevice.PresentationParameters.BackBufferHeight
);

// Detect if the sprite is contained within the bounds of the screen
if(!screenBounds.Contains(spriteBounds)) 
{
    // Respond by not allowing the sprite to move outside the screen
    // bounds by setting the location back to the previous location.
    newLocation = previousLocation;
}

_spriteLocation = newLocation;