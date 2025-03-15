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

    protected override void LoadContent()
    {
        // Load the game's background theme music.
        Song theme = Content.Load<Song>("audio/theme");

        // Start playing the background theme music using the audio controller.
        Audio.PlaySong(theme);

        base.LoadContent();        
    }
}
