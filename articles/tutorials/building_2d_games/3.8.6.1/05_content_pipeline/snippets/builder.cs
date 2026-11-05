public class Builder : ContentBuilder
{
  public override IContentCollection GetContentCollection()
  {
    var content = new ContentCollection();

    content.Include<WildcardRule>("images/*.png");

    // No content is imported by default.
    // Define your content collection rules here.
    //
    //   https://docs.monogame.net/articles/getting_started/content_pipeline/content_builder_project.html?#creating-your-contentcollection
    //
    // Examples:
    //
    // Import all content in the Assets folder using the default importer for their file type.
    // content.Include<WildcardRule>("*");
    //
    // Only copy content from the assets folder rather than build it with the pipeline.
    // content.IncludeCopy<WildcardRule>("*.json");
    // 
    // Exclude assets that match the pattern., only required overriding a default import behavior.
    // content.Exclude<WildcardRule>("Font/*.txt");
    //
    // Include a specific asset with processor parameters.
    // content.Include("Models/character.glb", new FbxImporter(),
    //    new MeshAnimatedModelProcessor()
    //    {
    //        Scale = 100.0f
    //    }
    // );

    return content;
  }
