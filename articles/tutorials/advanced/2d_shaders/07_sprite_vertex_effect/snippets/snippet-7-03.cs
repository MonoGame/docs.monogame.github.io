static VertexPositionColorTexture()
{
	var elements = new VertexElement[] 
	{ 
		new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0), 
		new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0), 
		new VertexElement(16, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0) 
	};
	VertexDeclaration = new VertexDeclaration(elements);
}
