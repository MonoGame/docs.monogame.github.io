private void InitializeLights()
{
    // ...
    
    var tileUnit = new Vector2(_tilemap.TileWidth, _tilemap.TileHeight);  
    var size = new Vector2(_tilemap.Columns, _tilemap.Rows);  
    _shadowCasters.Add(new ShadowCaster  
    {  
        Points = new List<Vector2>  
        {        tileUnit * new Vector2(1, 1),  
            tileUnit * new Vector2(size.X - 1, 1),  
            tileUnit * new Vector2(size.X - 1, size.Y - 1),  
            tileUnit * new Vector2(1, size.Y - 1),  
        }  
    });
}