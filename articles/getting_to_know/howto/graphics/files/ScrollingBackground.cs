using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// A helper class to scroll a texture vertically
/// </summary>
public class ScrollingBackground
{
    // Screen Position to draw the background from.
    private Vector2 screenpos;
    
    // The texture origin point.
    private Vector2 origin;
    
    // The size of the texture in pixels (using ony the texture height).
    private Vector2 texturesize;
    
    // The texture to draw as a background.
    private Texture2D mytexture;
    
    // The height of the screen in pixels.
    private int screenheight;

    public void Load(GraphicsDevice device, Texture2D backgroundTexture)
    {
        mytexture = backgroundTexture;
        screenheight = device.Viewport.Height;
        int screenwidth = device.Viewport.Width;

        // Set the origin so that we're drawing from the 
        // center of the top edge.
        origin = new Vector2(mytexture.Width / 2, 0);

        // Set the screen position to the center of the screen.
        screenpos = new Vector2(screenwidth / 2, screenheight / 2);

        // Offset to draw the second texture, when necessary.
        texturesize = new Vector2(0, mytexture.Height);
    }

    public void Update(float deltaY)
    {
        screenpos.Y += deltaY;
        screenpos.Y %= mytexture.Height;
    }

    public void Draw(SpriteBatch batch, Color color)
    {
        // Draw the texture, if it is still onscreen.
        if (screenpos.Y < screenheight)
        {
            batch.Draw(mytexture, screenpos, null,
                 color, 0, origin, 1, SpriteEffects.None, 0f);
        }
        
        // Draw the texture a second time, behind the first,
        // to create the scrolling illusion.
        batch.Draw(mytexture, screenpos - texturesize, null,
             color, 0, origin, 1, SpriteEffects.None, 0f);
    }
}