/// <summary>
/// An object that can be attached to a Game and have its Update method called when Game.Update is called.
/// </summary>
public class GameComponent : IGameComponent, IUpdateable, IDisposable
{
    /// <summary>
    /// Gets the Game that owns this GameComponent.
    /// </summary>
    public Game Game { get; private set; }

    /// <summary>
    /// Gets or Sets whether the Update method of this GameComponent should be called.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or Sets the order in which this GameComponent should be updated relative to other
    /// GameComponent instances.
    /// </summary>
    public int UpdateOrder { get; set; }

    /// <summary>
    /// Raised when Enabled is changed.
    /// </summary>
    public event EventHandler<EventArgs> EnabledChanged;

    /// <summary>
    /// Raised when UpdateOrder is changed.
    /// </summary>
    public event EventHandler<EventArgs> UpdateOrderChanged;

    /// <summary>
    /// Creates a GameComponent.
    /// </summary>
    /// <param name="game">The game that this component will belong to.</param>
    public GameComponent(Game game) { }

    /// <summary>
    /// Called when the GameComponent needs to be initialized.
    /// </summary>
    public virtual void Initialize() { }

    /// <summary>
    /// Updates this GameComponent.
    /// </summary>
    /// <param name="gameTime">The elapsed time since the last call to Update.</param>
    public virtual void Update(GameTime gameTime) { }

    /// <summary>
    /// Called when UpdateOrder is changed. Raises the UpdateOrderChanged event.
    /// </summary>
    /// <param name="sender">This GameComponent.</param>
    /// <param name="args">Arguments to the UpdateOrderChanged event.</param>
    protected virtual void OnUpdateOrderChanged(object sender, EventArgs args) { }

    /// <summary>
    /// Called when Enabled property is changed. Raises the EnabledChanged event.
    /// </summary>
    /// <param name="sender">This GameComponent.</param>
    /// <param name="args">Arguments to the EnableChanged event.</param>
    protected virtual void OnEnabledChanged(object sender, EventArgs args) { }

    /// <summary>
    /// Shuts down the GameComponent.
    /// </summary>
    public void Dispose() { } {}

    /// <summary>
    /// Shuts down the GameComponent.
    /// </summary>
    protected virtual void Dispose(bool disposing) { }
}
