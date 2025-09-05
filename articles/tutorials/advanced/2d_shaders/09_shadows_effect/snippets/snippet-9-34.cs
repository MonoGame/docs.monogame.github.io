public class ShadowCaster
{
    /// <summary>
    /// The position of the shadow caster
    /// </summary>
    public Vector2 Position;
    
    /// <summary>
    /// A list of at least 2 points that will be used to create a closed loop shape.
    /// The points are relative to the position.
    /// </summary>
    public List<Vector2> Points;
}
