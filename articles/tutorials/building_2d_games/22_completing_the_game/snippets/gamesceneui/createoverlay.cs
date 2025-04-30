private ColoredRectangleRuntime CreateOverlay()
{
    ColoredRectangleRuntime rect = new ColoredRectangleRuntime();
    rect = new ColoredRectangleRuntime();
    rect.Dock(Gum.Wireframe.Dock.Fill);
    rect.Red = 20;
    rect.Green = 23;
    rect.Blue = 47;
    rect.Alpha = 175;
    rect.Visible = false;

    return rect;
}