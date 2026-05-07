private void InitializeLights()
{
    // ...

    // Replace this with the new List approach below
    //_shadowCasters.Add(new ShadowCaster
    //{
    //     A = new Vector2(700, 320),
    //     B = new Vector2(700, 400)
    // });

    // simple shadow caster  
    _shadowCasters.Add(ShadowCaster.SimplePolygon(_slime.GetBounds().Location, radius: 30, sides: 6));
}
