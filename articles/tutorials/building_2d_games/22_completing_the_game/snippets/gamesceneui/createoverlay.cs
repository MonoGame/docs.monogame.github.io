private void CreateOverlay()
{
    _overlay = new ColoredRectangleRuntime();
    _overlay.Dock(Gum.Wireframe.Dock.Fill);
    _overlay.Red = 20;
    _overlay.Green = 23;
    _overlay.Blue = 47;
    _overlay.Alpha = 175;
    _overlay.Visible = false;
}