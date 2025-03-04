/// <summary>
/// Interface for updateable entities.
/// </summary>
public interface IUpdateable
{
    /// <summary>
    /// Gets whether Update should be called for this IUpdatable.
    /// </summary>
    bool Enabled { get; }

    /// <summary>
    /// Gets the order in which this IUpdateable should be updated relative to other IUpdateable instances.
    /// </summary>
    int UpdateOrder { get; }

    /// <summary>
    /// Raised when Enabled is changed.
    /// </summary>
    event EventHandler<EventArgs> EnabledChanged;

    /// <summary>
    /// Raised when UpdateOrder changed.
    /// </summary>
    event EventHandler<EventArgs> UpdateOrderChanged;

    /// <summary>
    /// Called when this IUpdatable should update itself.
    /// </summary>
    /// <param name="gameTime">The elapsed time since the last call to Update.</param>
    void Update(GameTime gameTime);
}
