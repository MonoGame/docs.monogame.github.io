public class Builder : ContentBuilder
{
  public override IContentCollection GetContentCollection()
  {
    var content = new ContentCollection();

    content.Include<WildcardRule>("images/*.png");

    // Copy XML files from the assets folder rather than build them with the pipeline.
    content.IncludeCopy<WildcardRule>("images/*.xml");

    // Include all audio from the audio folder.
    // Each file is built using its default processor and importer: 
    // SoundEffect for .wav files, Song for .ogg, .mp3, and .wma files.
    content.Include<WildcardRule>("audio/*");

    // Include any SpriteFont Description files from the fonts folder in our build.
    content.Include<WildcardRule>("fonts/*.spritefont");

    // Exclude TrueType font files so the builder doesn't try to build both .ttf and .spritefont files.
    content.Exclude<WildcardRule>("fonts/*.ttf");

    return content;
  }
}
