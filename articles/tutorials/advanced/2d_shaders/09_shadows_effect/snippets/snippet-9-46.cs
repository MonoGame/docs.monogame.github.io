public Bat(AnimatedSprite sprite, SoundEffect bounceSoundEffect)
{
    // ...
    
    ShadowCaster = ShadowCaster.SimplePolygon(Point.Zero, radius: 10, sides: 12);
}
