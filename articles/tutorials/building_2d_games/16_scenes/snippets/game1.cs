using DungeonSlime.Scenes;
using MonoGameLibrary;

namespace DungeonSlime;

public class Game1 : Core
{
    public Game1() : base("Dungeon Slime", 1280, 720, false)
    {

    }

    protected override void Initialize()
    {
        base.Initialize();

        // Start the game with the title scene.
        ChangeScene(new TitleScene());
    }
}
