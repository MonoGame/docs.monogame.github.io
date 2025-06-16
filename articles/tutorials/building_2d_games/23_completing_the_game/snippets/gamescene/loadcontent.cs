public override void LoadContent()
{
    // Create the texture atlas from the XML configuration file.
    TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/atlas-definition.xml");

    // Create the tilemap from the XML configuration file.
    _tilemap = Tilemap.FromFile(Content, "images/tilemap-definition.xml");
    _tilemap.Scale = new Vector2(4.0f, 4.0f);

    // Create the animated sprite for the slime from the atlas.
    AnimatedSprite slimeAnimation = atlas.CreateAnimatedSprite("slime-animation");
    slimeAnimation.Scale = new Vector2(4.0f, 4.0f);

    // Create the slime.
    _slime = new Slime(slimeAnimation);

    // Create the animated sprite for the bat from the atlas.
    AnimatedSprite batAnimation = atlas.CreateAnimatedSprite("bat-animation");
    batAnimation.Scale = new Vector2(4.0f, 4.0f);

    // Load the bounce sound effect for the bat.
    SoundEffect bounceSoundEffect = Content.Load<SoundEffect>("audio/bounce");

    // Create the bat.
    _bat = new Bat(batAnimation, bounceSoundEffect);

    // Load the collect sound effect.
    _collectSoundEffect = Content.Load<SoundEffect>("audio/collect");
}