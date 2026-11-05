public override void Initialize()
{
    // LoadContent is called during base.Initialize().
    base.Initialize();

    // During the game scene, we want to disable exit on escape. Instead,
    // the escape key will be used to return back to the title screen.
    Core.ExitOnEscape = false;

    // Create the room bounds by getting the bounds of the screen then
    // using the Inflate method to "Deflate" the bounds by the width and
    // height of a tile so that the bounds only covers the inside room of
    // the dungeon tilemap.
    _roomBounds = Core.GraphicsDevice.PresentationParameters.Bounds;
    _roomBounds.Inflate(-_tilemap.TileWidth, -_tilemap.TileHeight);

    // Subscribe to the slime's BodyCollision event so that a game over
    // can be triggered when this event is raised.
    _slime.BodyCollision += OnSlimeBodyCollision;

    // Create any UI elements from the root element created in previous
    // scenes.
    GumService.Default.Root.Children.Clear();

    // Initialize the user interface for the game scene.
    InitializeUI();

    // Initialize a new game to be played.
    InitializeNewGame();
}