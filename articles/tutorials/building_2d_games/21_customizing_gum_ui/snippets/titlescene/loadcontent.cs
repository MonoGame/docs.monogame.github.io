public override void LoadContent()
{
    // Load the font for the standard text.
    _font = Core.Content.Load<SpriteFont>("fonts/04B_30");

    // Load the font for the title text
    _font5x = Content.Load<SpriteFont>("fonts/04B_30_5x");

    // Load the background pattern texture.
    _backgroundPattern = Content.Load<Texture2D>("images/background-pattern");

    // Load the sound effect to play when ui actions occur.
    _uiSoundEffect = Core.Content.Load<SoundEffect>("audio/ui");

    // Load the texture atlas from the xml configuration file.
    _atlas = TextureAtlas.FromFile(Core.Content, "images/atlas-definition.xml");
}