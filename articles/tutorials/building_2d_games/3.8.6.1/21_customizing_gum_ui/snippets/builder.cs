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

    // Include the SpriteFont Description file in our build.
    content.Include<WildcardRule>("fonts/*.spritefont");

    // Exclude TrueType font files so the builder doesn't try to build both the .ttf and the .spritefont file.
    content.Exclude<WildcardRule>("fonts/*.ttf");

    // Copy .fnt font files rather than building them with the pipeline.
    content.IncludeCopy<WildcardRule>("fonts/*.fnt");

    return content;
  }
}
