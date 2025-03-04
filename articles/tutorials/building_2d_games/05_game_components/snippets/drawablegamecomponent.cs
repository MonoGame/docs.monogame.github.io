/// <summary>
/// An object that can be attached to a Game and have its Draw method called when Game.Draw is called.
/// </summary>
/// <remarks>
/// This inherits from GameComponent, so it's Update method will also be called when Game.Update is called.
/// </remarks>
public class DrawableGameComponent : GameComponent, IDrawable
{
    /// <summary>
    /// Gets the GraphicsDevice that this DrawableGameComponent uses for drawing.
    /// </summary>
    public GraphicsDevice GraphicsDevice { get; }

    /// <summary>
    /// Gets or Sets the order in which this DrawableGameComponent should be drawn relative to other
    /// DrawableGameComponent instances.
    /// </summary>
    public int DrawOrder { get; set; }

    /// <summary>
    /// Gets or Sets a value that indicates whether the Draw method of this DrawableGameComponent should be called.
    /// </summary>
    public bool Visible { get; set; }

    /// <summary>
    /// Raised when the DrawOrder property is changed.
    /// </summary>
    public event EventHandler<EventArgs> DrawOrderChanged;

    /// <summary>
    /// Raised when the Visible property is changed.
    /// </summary>
    public event EventHandler<EventArgs> VisibleChanged;

    /// <summary>
    /// Creates a DrawableGameComponent.
    /// </summary>
    /// <param name="game">The game that this component will belong to.</param>
    public DrawableGameComponent(Game game) : base(game) { }

    /// <summary>
    /// Called when the DrawableGameComponent needs to be initialized.
    /// </summary>
    public override void Initialize() { }

    /// <summary>
    /// Loads graphical resources needed by this DrawableGameComponent.
    /// </summary>
    protected virtual void LoadContent() { }

    /// <summary>
    /// Unloads graphical resources needed by this DrawableGameComponent.
    /// </summary>
    protected virtual void UnloadContent() { }

    /// <summary>
    /// Draws this DrawableGameComponent.
    /// </summary>
    /// <param name="gameTime">The time elapsed since the last call to Draw.</param>
    public virtual void Draw(GameTime gameTime) { }

    /// <summary>
    /// Called when the Visible property is changed. Raises the VisibleChanged event.
    /// </summary>
    /// <param name="sender">This DrawableGameComponent.</param>
    /// <param name="args">Arguments to the VisibleChanged event.</param>
    protected virtual void OnVisibleChanged(object sender, EventArgs args) { }

    /// <summary>
    /// Called when DrawOrder property is changed.
    /// </summary>
    /// <param name="sender">This DrawableGameComponent.</param>
    /// <param name="args">Arguments to the DrawOrderChanged event.</param>
    protected virtual void OnDrawOrderChanged(object sender, EventArgs args)

    /// <summary>
    /// Shuts down the DrawableGameComponent.
    /// </summary>
    protected override void Dispose(bool disposing) { }
}