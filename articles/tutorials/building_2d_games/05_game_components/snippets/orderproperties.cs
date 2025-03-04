public class ExampleComponent : DrawableGameComponent
{
    public ExampleComponent(Game game) : base(game)
    {
        // Update after components with order 0.
        UpdateOrder = 1;

        // Draw after components with order 0.
        DrawOrder = 1;
    }
}