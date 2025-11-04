private void InitializeLights()
{
    // ...
    
    // simple shadow caster  
    _shadowCasters.Add(ShadowCaster.SimplePolygon(_slime.GetBounds().Location, radius: 30, sides: 6));
}
