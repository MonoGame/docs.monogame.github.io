private void InitializeUI()
{
    // Clear out any previous UI in case we came here from
    // a different screen:
    Gum.GumService.Default.Root.Children.Clear();

    CreateTitlePanel();
    CreateOptionsPanel();
}