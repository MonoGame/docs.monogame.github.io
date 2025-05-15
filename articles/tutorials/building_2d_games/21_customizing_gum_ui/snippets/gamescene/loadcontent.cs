public override void LoadContent()
{
    // Create the texture atlas from the XML configuration file
    _atlas = TextureAtlas.FromFile(Core.Content, "images/atlas-definition.xml");

    // Create the slime animated sprite from the atlas.
    _slime = _atlas.CreateAnimatedSprite("slime-animation");
    _slime.Scale = new Vector2(4.0f, 4.0f);

    // Create the bat animated sprite from the atlas.
    _bat = _atlas.CreateAnimatedSprite("bat-animation");
    _bat.Scale = new Vector2(4.0f, 4.0f);

    // Create the tilemap from the XML configuration file.
    _tilemap = Tilemap.FromFile(Content, "images/tilemap-definition.xml");
    _tilemap.Scale = new Vector2(4.0f, 4.0f);

    // Load the bounce sound effect
    _bounceSoundEffect = Content.Load<SoundEffect>("audio/bounce");

    // Load the collect sound effect
    _collectSoundEffect = Content.Load<SoundEffect>("audio/collect");

    // Load the font
    _font = Core.Content.Load<SpriteFont>("fonts/04B_30");

    // Load the sound effect to play when ui actions occur.
    _uiSoundEffect = Core.Content.Load<SoundEffect>("audio/ui");
}