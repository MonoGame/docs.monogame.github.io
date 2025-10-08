protected override void LoadContent()  
{  
    // Allow the Core class to also load content.
    base.LoadContent();  
    // Load the background theme music.
    _themeSong = Content.Load<Song>("audio/theme");  
}
