/// <summary>
/// Entry point for your Content Builder project.
/// When run it builds your content according to the content collection.
/// </summary>
/// <remarks>
/// For more details regarding the Content Builder, see the MonoGame documentation:
///
///    https://docs.monogame.net/articles/getting_started/content_pipeline/content_builder_project.html
///
/// </remarks>

using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Framework.Content.Pipeline.Builder;


// If you need to debug the content build process you can enable
// this, build your game, then attach the debugger when prompted.
//System.Diagnostics.Debugger.Launch();


var builder = new Builder();

// We depend on the build arguments from the command line which
// are typically passed from your game project build commands.
if (args.Length > 0)
  builder.Run(args);

else
{
  // NOTE: You could create your own ContentBuilderParams
  // and configure it and pass it to Run if you don't want
  // to depend on the caller passing the right arguments.

  // Calling this will just return the help message.
  builder.Run(new ContentBuilderParams { Mode = ContentBuilderMode.None });
}

return builder.FailedToBuild > 0 ? -1 : 0;

#region class-def
public class Builder : ContentBuilder
{
  public override IContentCollection GetContentCollection()
  {
    var content = new ContentCollection();

    // Build .png images in the images folder with the pipeline.
    content.Include<WildcardRule>("images/*.png");

    // Copy XML files from the assets folder rather than build them with the pipeline.
    content.IncludeCopy<WildcardRule>("images/*.xml");

    return content;
  }
}
#endregion

