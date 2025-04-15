using Microsoft.Xna.Framework;

namespace DungeonSlime;

public struct SlimeSegment
{
    /// <summary>
    /// The position this slime segment is at before the next tick occurs.
    /// </summary>
    public Vector2 At;

    /// <summary>
    /// The position this slime segment is visually moving toward.
    /// </summary>
    public Vector2 To;

    /// <summary>
    /// The direction this slime segment is moving.
    /// </summary>
    public Vector2 Direction;

    /// <summary>
    /// The opposite direction this slime segment is moving.
    /// </summary>
    public Vector2 ReverseDirection => new Vector2(-Direction.X, -Direction.Y);
}
