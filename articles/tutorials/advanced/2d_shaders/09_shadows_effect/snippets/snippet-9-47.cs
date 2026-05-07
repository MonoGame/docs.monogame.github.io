public void Update(GameTime gameTime)
{
    // ...
    
    // Update the position of the shadow caster. Move it up a bit due to the bat's artwork.  
    var size = new Vector2(_sprite.Width, _sprite.Height);
    ShadowCaster.Position = Position - Vector2.UnitY * 10 + size * .5f;
}
