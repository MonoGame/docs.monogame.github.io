using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace DungeonSlime;

public class Game1 : Core
{
  private GraphicsDeviceManager _graphics;
  private SpriteBatch _spriteBatch;

  public Game1() : base("Dungeon Slime", 1280, 720, false)
  {

  }

  protected override void Initialize()
  {
    // TODO: Add your initialization logic here

    base.Initialize();
  }

  protected override void LoadContent()
  {
    // TODO: use this.Content to load your game content here
  }

  protected override void Update(GameTime gameTime)
  {
#if !__IOS__
    // Exit the game if the Back button (GamePad) or Escape key (Keyboard) is pressed.
    // (iOS does not allow programmatically closing an app.)
    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
      Exit();
#endif

    // TODO: Add your update logic here

    base.Update(gameTime);
  }

  protected override void Draw(GameTime gameTime)
  {
    GraphicsDevice.Clear(Color.CornflowerBlue);

    // TODO: Add your drawing code here

    base.Draw(gameTime);
  }
}
