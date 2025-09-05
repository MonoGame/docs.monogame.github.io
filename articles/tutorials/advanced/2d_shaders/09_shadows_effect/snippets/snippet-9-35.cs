public static ShadowCaster SimplePolygon(Point position, float radius, int sides)
{
	var anglePerSide = MathHelper.TwoPi / sides;
	var caster = new ShadowCaster
	{
		Position = position.ToVector2(),
		Points = new List<Vector2>(sides)
	};
	for (var angle = 0f; angle < MathHelper.TwoPi; angle += anglePerSide)
	{
		var pt = radius * new Vector2(MathF.Cos(angle), MathF.Sin(angle));
		caster.Points.Add(pt);
	}

	return caster;
}
