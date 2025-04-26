protected override void Initialize()
{
   base.Initialize();

   // Start playing the background music
   Audio.PlaySong(_themeSong);

   // Initialize the Gum UI service
   InitializeGum();

   // Start the game with the title scene.
   ChangeScene(new TitleScene());
}