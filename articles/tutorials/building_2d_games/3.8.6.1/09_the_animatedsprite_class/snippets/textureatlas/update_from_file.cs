/// <summary>
/// Creates a new texture atlas based a texture atlas xml configuration file.
/// </summary>
/// <param name="content">The content manager used to load the texture for the atlas.</param>
/// <param name="fileName">The path to the xml file, relative to the content root directory..</param>
/// <returns>The texture atlas created by this method.</returns>
public static TextureAtlas FromFile(ContentManager content, string fileName)
{
    TextureAtlas atlas = new TextureAtlas();

    string filePath = Path.Combine(content.RootDirectory, fileName);

    using (Stream stream = TitleContainer.OpenStream(filePath))
    {
        using (XmlReader reader = XmlReader.Create(stream))
        {
            XDocument doc = XDocument.Load(reader);
            XElement root = doc.Root;

            // The <Texture> element contains the content path for the Texture2D to load.
            // So we will retrieve that value then use the content manager to load the texture.
            string texturePath = root.Element("Texture").Value;
            atlas.Texture = content.Load<Texture2D>(texturePath);

            // The <Regions> element contains individual <Region> elements, each one describing
            // a different texture region within the atlas.  
            //
            // Example:
            // <Regions>
            //      <Region name="spriteOne" x="0" y="0" width="32" height="32" />
            //      <Region name="spriteTwo" x="32" y="0" width="32" height="32" />
            // </Regions>
            //
            // So we retrieve all of the <Region> elements then loop through each one
            // and generate a new TextureRegion instance from it and add it to this atlas.
            var regions = root.Element("Regions")?.Elements("Region");

            if (regions != null)
            {
                foreach (var region in regions)
                {
                    string name = region.Attribute("name")?.Value;
                    int x = int.Parse(region.Attribute("x")?.Value ?? "0");
                    int y = int.Parse(region.Attribute("y")?.Value ?? "0");
                    int width = int.Parse(region.Attribute("width")?.Value ?? "0");
                    int height = int.Parse(region.Attribute("height")?.Value ?? "0");

                    if (!string.IsNullOrEmpty(name))
                    {
                        atlas.AddRegion(name, x, y, width, height);
                    }
                }
            }

            // The <Animations> element contains individual <Animation> elements, each one describing
            // a different animation within the atlas.
            //
            // Example:
            // <Animations>
            //      <Animation name="animation" delay="100">
            //          <Frame region="spriteOne" />
            //          <Frame region="spriteTwo" />
            //      </Animation>
            // </Animations>
            //
            // So we retrieve all of the <Animation> elements then loop through each one
            // and generate a new Animation instance from it and add it to this atlas.
            var animationElements = root.Element("Animations").Elements("Animation");

            if (animationElements != null)
            {
                foreach (var animationElement in animationElements)
                {
                    string name = animationElement.Attribute("name")?.Value;
                    float delayInMilliseconds = float.Parse(animationElement.Attribute("delay")?.Value ?? "0");
                    TimeSpan delay = TimeSpan.FromMilliseconds(delayInMilliseconds);

                    List<TextureRegion> frames = new List<TextureRegion>();

                    var frameElements = animationElement.Elements("Frame");

                    if (frameElements != null)
                    {
                        foreach (var frameElement in frameElements)
                        {
                            string regionName = frameElement.Attribute("region").Value;
                            TextureRegion region = atlas.GetRegion(regionName);
                            frames.Add(region);
                        }
                    }

                    Animation animation = new Animation(frames, delay);
                    atlas.AddAnimation(name, animation);
                }
            }

            return atlas;
        }
    }
}