protected override void LoadContent()  
{  
    // Allow the Core class to load any content.  
    base.LoadContent();  
    // Load the background theme music  
    _themeSong = Content.Load<Song>("audio/theme");  
}
