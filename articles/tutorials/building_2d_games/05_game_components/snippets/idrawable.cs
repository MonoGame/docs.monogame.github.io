/// <summary>
/// Interface for drawable entities.
/// </summary>
public interface IDrawable
{
    /// <summary>
    /// Gets whether Draw should be called for this IDrawable.
    /// </summary>
    bool Visible { get; }

    /// <summary>
    /// Gets the order in which this IDrawable should be drawn relative to other IDrawable instances.
    /// </summary>
    int DrawOrder { get; }

    /// <summary>
    /// Raised when the Visible property is changed.
    /// </summary>
    event EventHandler<EventArgs> VisibleChanged;

    /// <summary>
    /// Raised when the DrawOrder property is changed.
    /// </summary>
    event EventHandler<EventArgs> DrawOrderChanged;

    /// <summary>
    /// Called when this IDrawable should draw itself.
    /// </summary>
    /// <param name="gameTime">The elapsed time since the last call to Draw.</param>
    void Draw(GameTime gameTime);
}