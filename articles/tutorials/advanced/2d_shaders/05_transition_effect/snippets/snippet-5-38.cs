public Core(string title, int width, int height, bool fullScreen)
{
    // ...

    // Set the core's content manager to a reference of hte base Game's
    // content manager.
    Content = base.Content;

    // Set the root directory for content
    Content.RootDirectory = "Content";

    // Set the core's shared content manager, pointing to the SharedContent folder.
    SharedContent = new ContentManager(Services, "SharedContent");

    // ...
}