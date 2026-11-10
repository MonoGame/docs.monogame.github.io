// Store the current location.
Vector2 previousLocation = _spriteLocation;

// Calculate a new location.
Vector2 newLocation = _spriteLocation + new Vector2(10, 0);

// Create a bounding box for the sprite object.
Rectangle spriteBounds = new Rectangle(
    (int)newLocation.X,
    (int)newLocation.Y,
    (int)_sprite.Width,
    (int)_sprite.Height
);

// Create a bounding box for the blocking object.
Rectangle blockingBounds = new Rectangle(
    (int)_blockingLocation_.X,
    (int)_blockingLocation_.Y,
    (int)_blockingSprite_.Width,
    (int)_blockingSprite_.Height
);

// Detect if they are colliding
if(spriteBounds.Intersects(blockingBounds)) 
{
    // Respond by not allowing the sprite to move by setting
    // the location back to the previous location.
    newLocation = previousLocation;
}

_spriteLocation = newLocation;