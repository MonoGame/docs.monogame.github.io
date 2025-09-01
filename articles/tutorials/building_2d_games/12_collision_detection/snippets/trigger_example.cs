// Create a bounding box for the sprite object.
Rectangle spriteBounds = new Rectangle(
    (int)_spriteLocation.X,
    (int)_spriteLocation.Y,
    (int)_sprite.Width,
    (int)_sprite.Height
);

// Detect if the sprite object is within the trigger zone.
if(_spriteBounds.Intersects(_triggerBounds))
{
    // Perform some event.
    CollectItem();
}