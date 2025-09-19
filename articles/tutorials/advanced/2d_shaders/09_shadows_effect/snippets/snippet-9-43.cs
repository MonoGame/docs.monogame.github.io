public void Update(GameTime gameTime)
{
	// ...
	
	// Update the shadow casters
	if (ShadowCasters.Count != _segments.Count)
	{
		ShadowCasters = new List<ShadowCaster>(_segments.Count);
		for (var i = 0; i < _segments.Count; i++)
		{
			ShadowCasters.Add(ShadowCaster.SimplePolygon(Point.Zero, radius: 30, sides: 12));
		}
	}

	// move the shadow casters to the current segment positions
	for (var i = 0; i < _segments.Count; i++)
	{
		var segment = _segments[i];
		Vector2 pos = Vector2.Lerp(segment.At, segment.To, _movementProgress);
		var size = new Vector2(_sprite.Width, _sprite.Height);
		ShadowCasters[i].Position = pos + size * .5f;
	}

}