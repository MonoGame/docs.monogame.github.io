using DungeonSlime.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.UI;

namespace DungeonSlime.Scenes;

public class TitleScene : Scene
{
    // The texture used for the background pattern.
    private Texture2D _backgroundPattern;

    // The destination rectangle for the background pattern to fill.
    private Rectangle _backgroundDestination;

    // The offset to apply when drawing the background pattern so it appears to
    // be scrolling.
    private Vector2 _backgroundOffset;

    // The speed that the background pattern scrolls.
    private float _scrollSpeed = 50.0f;

    // Tracks the ui element that represents the title menu.
    private UIElement _titleMenu;

    // Tracks the ui element that represents the options menu.
    private UIElement _optionsMenu;

    // Tracks the volume level of songs before changes are made in the options
    // menu so it can be restored if they player selects cancel.
    private float _previousSongVolume;

    // Tracks the volume level of sound effects before changes are made in the
    // options menu so it can be restored if the player selects cancel.
    private float _previousSoundEffectVolume;

    public override void Initialize()
    {
        base.Initialize();

        // Explicitly set exit on escape to false so that the game doesn't exit
        // when the options menu is open and the user presses escape to cancel
        // an action.  Instead, we'll perform the exit check in the title menu's
        // cancel logic
        Core.ExitOnEscape = false;

        // Initialize the offset of the background pattern at zero
        _backgroundOffset = Vector2.Zero;

        // Set the background pattern destination rectangle to fill the entire
        // screen background
        _backgroundDestination = Core.GraphicsDevice.PresentationParameters.Bounds;
    }

    public override void LoadContent()
    {
        // Create a texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/atlas-definition.xml");

        // Load the background pattern texture.
        _backgroundPattern = Content.Load<Texture2D>("images/background-pattern");

        // Load the sound effect to play when ui actions occur.
        SoundEffect uiSoundEffect = Core.Content.Load<SoundEffect>("audio/ui");

        // Create the UI controller
        UIElementController controller = new UIElementController();

        // Create the title menu
        CreateTileMenu(atlas, uiSoundEffect, controller);

        // Create the options menu
        CreateOptionsMenu(atlas, uiSoundEffect, controller);
    }

    private void CreateTileMenu(TextureAtlas atlas, SoundEffect soundEffect, UIElementController controller)
    {
        // Create the root container for the title menu.
        _titleMenu = new UISprite();
        _titleMenu.Position = Vector2.Zero;
        _titleMenu.Controller = controller;
        _titleMenu.IsSelected = true;

        // Create the title sprite as a child of the title menu.
        UISprite titleSprite = _titleMenu.CreateChild<UISprite>();
        titleSprite.Sprite = atlas.CreateSprite("title");
        titleSprite.Sprite.CenterOrigin();
        titleSprite.Position = new Vector2(640, 220);

        // Create the start button as a child of the title menu.
        UIButton startButton = _titleMenu.CreateChild<UIButton>();
        startButton.NotSelectedSprite = atlas.CreateSprite("start-button");
        startButton.NotSelectedSprite.CenterOrigin();
        startButton.SelectedSprite = atlas.CreateAnimatedSprite("start-button-selected");
        startButton.SelectedSprite.CenterOrigin();
        startButton.Position = new Vector2(432, 670);
        startButton.IsSelected = true;

        // Create the options button as a child of the title menu.
        UIButton optionsButton = _titleMenu.CreateChild<UIButton>();
        optionsButton.NotSelectedSprite = atlas.CreateSprite("options-button");
        optionsButton.NotSelectedSprite.CenterOrigin();
        optionsButton.SelectedSprite = atlas.CreateAnimatedSprite("options-button-selected");
        optionsButton.SelectedSprite.CenterOrigin();
        optionsButton.Position = new Vector2(848, 670);

        // Wire up the actions to perform when the Left action is triggered
        // for the menu.
        _titleMenu.LeftAction = () =>
        {
            // Play the sound effect.
            Core.Audio.PlaySoundEffect(soundEffect);

            // The left action was performed, so deselect the options button and
            // select the start button.
            startButton.IsSelected = true;
            optionsButton.IsSelected = false;
        };

        // Wire up the actions to perform when the Right action is triggered
        // for the menu.
        _titleMenu.RightAction = () =>
        {
            // Play the sound effect.
            Core.Audio.PlaySoundEffect(soundEffect);

            // The right action was performed, so deselect the start button and
            // select the options button.
            startButton.IsSelected = false;
            optionsButton.IsSelected = true;
        };

        // Wire up the actions to perform when the Confirm action is triggered
        // for the menu.
        _titleMenu.ConfirmAction = () =>
        {
            // Play the sound effect.
            Core.Audio.PlaySoundEffect(soundEffect);

            if (startButton.IsSelected)
            {
                // The start button is selected and the confirm action was
                // performed, so change the scene to the game select scene.
                Core.ChangeScene(new GameScene());
            }
            else if (optionsButton.IsSelected)
            {
                // The accept button is selected and the confirm action was
                // performed, so disable the title menu...
                _titleMenu.IsEnabled = _titleMenu.IsVisible = _titleMenu.IsSelected = false;

                // ...cache the current volume levels of songs and
                // sound effects..
                _previousSongVolume = Core.Audio.SongVolume;
                _previousSoundEffectVolume = Core.Audio.SoundEffectVolume;

                // ...and restore the default selections of the title menu for
                // when they come back to it...
                startButton.IsSelected = true;
                optionsButton.IsSelected = false;

                // ...and enable the options menu.
                _optionsMenu.IsEnabled = _optionsMenu.IsVisible = _optionsMenu.IsSelected = true;
            }
        };

        // If the cancel button is pressed while the title menu is active, tell
        // the game to exit.
        _titleMenu.CancelAction = Core.Instance.Exit;
    }

    private void CreateOptionsMenu(TextureAtlas atlas, SoundEffect soundEffect, UIElementController controller)
    {
        // Crete the root container for the options menu.
        _optionsMenu = new UIElement();
        _optionsMenu.Controller = controller;
        _optionsMenu.IsEnabled = false;
        _optionsMenu.IsVisible = false;

        // Create the options label as a child of the options menu.
        UISprite optionsLabel = _optionsMenu.CreateChild<UISprite>();
        optionsLabel.Sprite = atlas.CreateSprite("options-label");
        optionsLabel.Position = new Vector2(112, 20);

        // Create the enter label as a child of the options menu.
        UISprite enterLabel = _optionsMenu.CreateChild<UISprite>();
        enterLabel.Sprite = atlas.CreateSprite("enter-label");
        enterLabel.Position = new Vector2(640, 52);

        // Create the escape label as a child of the options menu.
        UISprite escapeLabel = _optionsMenu.CreateChild<UISprite>();
        escapeLabel.Sprite = atlas.CreateSprite("escape-label");
        escapeLabel.Position = new Vector2(804, 52);

        // Create the music panel as a child of the options menu.
        UISprite musicPanel = _optionsMenu.CreateChild<UISprite>();
        musicPanel.Sprite = atlas.CreateSprite("panel");
        musicPanel.Position = new Vector2(198, 139);
        musicPanel.IsSelected = true;

        // Create the sound effect panel as a child of the options menu.
        UISprite soundEffectPanel = _optionsMenu.CreateChild<UISprite>();
        soundEffectPanel.Sprite = atlas.CreateSprite("panel");
        soundEffectPanel.Position = new Vector2(198, 406);
        soundEffectPanel.IsEnabled = false;

        // Create the accept button as a child of the options menu.
        UIButton acceptButton = _optionsMenu.CreateChild<UIButton>();
        acceptButton.NotSelectedSprite = atlas.CreateSprite("accept-button");
        acceptButton.NotSelectedSprite.CenterOrigin();
        acceptButton.SelectedSprite = atlas.CreateAnimatedSprite("accept-button-selected");
        acceptButton.SelectedSprite.CenterOrigin();
        acceptButton.Position = new Vector2(432, 670);

        // Create the cancel button as a child of the options menu.
        UIButton cancelButton = _optionsMenu.CreateChild<UIButton>();
        cancelButton.NotSelectedSprite = atlas.CreateSprite("cancel-button");
        cancelButton.NotSelectedSprite.CenterOrigin();
        cancelButton.SelectedSprite = atlas.CreateAnimatedSprite("cancel-button-selected");
        cancelButton.SelectedSprite.CenterOrigin();
        cancelButton.Position = new Vector2(848, 670);

        // Create the music text as a child of the music panel.
        UISprite musicText = musicPanel.CreateChild<UISprite>();
        musicText.Sprite = atlas.CreateSprite("music-label");
        musicText.Position = new Vector2(42, 42);

        // Create the music volume slider as a child of the music panel.
        UISlider musicVolumeSlider = musicPanel.CreateChild<UISlider>();
        musicVolumeSlider.SliderSprite = atlas.CreateSprite("slider");
        musicVolumeSlider.FillSprite = atlas.CreateSprite("white-pixel");
        musicVolumeSlider.FillBounds = new Rectangle(108, 4, 566, 36);
        musicVolumeSlider.Value = Core.Audio.SongVolume;
        musicVolumeSlider.MinValue = 0.0f;
        musicVolumeSlider.MaxValue = 1.0f;
        musicVolumeSlider.Step = 0.1f;
        musicVolumeSlider.Position = new Vector2(27, 117);

        // Create the sound text as a child of the sound effect panel.
        UISprite soundText = soundEffectPanel.CreateChild<UISprite>();
        soundText.Sprite = atlas.CreateSprite("sound-effect-label");
        soundText.Position = new Vector2(42, 42);

        // Create the sound effect volume slider as a child of the sound effect panel.
        UISlider soundEffectVolumeSlider = soundEffectPanel.CreateChild<UISlider>();
        soundEffectVolumeSlider.SliderSprite = atlas.CreateSprite("slider");
        soundEffectVolumeSlider.FillSprite = atlas.CreateSprite("white-pixel");
        soundEffectVolumeSlider.FillBounds = new Rectangle(108, 4, 566, 36);
        soundEffectVolumeSlider.Value = Core.Audio.SoundEffectVolume;
        soundEffectVolumeSlider.MinValue = 0.0f;
        soundEffectVolumeSlider.MaxValue = 1.0f;
        soundEffectVolumeSlider.Step = 0.1f;
        soundEffectVolumeSlider.Position = new Vector2(27, 117);

        // By setting the disabled color of the root options menu, it will
        // propagate the value to all child elements
        _optionsMenu.DisabledColor = new Color(70, 86, 130, 255);

        // Wire up the actions to perform when the Up Action is triggered
        // for the menu.
        _optionsMenu.UpAction = () =>
        {
            // Play the sound effect
            Core.Audio.PlaySoundEffect(soundEffect);

            if (soundEffectPanel.IsSelected)
            {
                // The sound effect panel is selected and the up action was
                // performed, so deselect the sound effect panel and move the
                // navigation up to the music panel.
                soundEffectPanel.IsEnabled = soundEffectPanel.IsSelected = false;
                musicPanel.IsEnabled = musicPanel.IsSelected = true;
            }
            else if (acceptButton.IsSelected)
            {
                // The accept button is selected and the up action was
                // performed, so deselect the accept button and move the
                // navigation up to the sound effect panel.
                acceptButton.IsSelected = false;
                soundEffectPanel.IsEnabled = soundEffectPanel.IsSelected = true;
            }
            else if (cancelButton.IsSelected)
            {
                // The cancel button is selected and the up action was
                // performed, so deselect the cancel button and move the
                // navigation up to the sound effect panel.
                cancelButton.IsSelected = false;
                soundEffectPanel.IsEnabled = soundEffectPanel.IsSelected = true;
            }
        };

        // Wire up the actions to perform when the Down action is triggered
        // for the menu.
        _optionsMenu.DownAction = () =>
        {
            // Play the sound effect
            Core.Audio.PlaySoundEffect(soundEffect);

            if (musicPanel.IsSelected)
            {
                // The music panel is selected and the down action was
                // performed, so deselect the music panel and move the
                // navigation down to the  sound effect panel.
                musicPanel.IsEnabled = musicPanel.IsSelected = false;
                soundEffectPanel.IsEnabled = soundEffectPanel.IsSelected = true;
            }
            else if (soundEffectPanel.IsSelected)
            {
                // The sound effect panel is selected and the down action was
                // performed, so deselect the sound effect panel and move the
                // navigation down to the accept button.
                soundEffectPanel.IsEnabled = soundEffectPanel.IsSelected = false;
                acceptButton.IsSelected = true;
            }
        };

        // Wire up the actions to perform when the Left action is triggered
        // for the menu.
        _optionsMenu.LeftAction = () =>
        {
            if (musicPanel.IsSelected)
            {
                // The music panel is selected and the right action was
                // performed, so step down the music volume slider.
                Core.Audio.SongVolume = musicVolumeSlider.StepDown();
            }
            else if (soundEffectPanel.IsSelected)
            {
                // The sound effect panel is selected and the right action was
                // performed, so step down the sound effect volume slider.
                Core.Audio.SoundEffectVolume = soundEffectVolumeSlider.StepDown();

            }
            else if (cancelButton.IsSelected)
            {
                // The cancel button is selected and the right action was
                // performed, so deselect the cancel button and move the
                // navigation to the accept button.
                cancelButton.IsSelected = false;
                acceptButton.IsSelected = true;
            }

            // For this action, we'll play the sound effect at the end of the
            // action so that if the sound effect volume slider was adjusted
            // the sound effect played will have those changes.
            Core.Audio.PlaySoundEffect(soundEffect);
        };

        // Wire up the actions to perform when the Right action is triggered
        // for the menu.
        _optionsMenu.RightAction = () =>
        {
            if (musicPanel.IsSelected)
            {
                // The music panel is selected and the right action was
                // performed, so step up the music volume slider.
                Core.Audio.SongVolume = musicVolumeSlider.StepUp();

                // Play the sound effect
                Core.Audio.PlaySoundEffect(soundEffect);
            }
            else if (soundEffectPanel.IsSelected)
            {
                // The sound effect panel is selected and the right action was
                // performed, so step up the sound effect volume slider.
                Core.Audio.SoundEffectVolume = soundEffectVolumeSlider.StepUp();

            }
            else if (acceptButton.IsSelected)
            {
                // The accept button is selected and the right action was
                // performed, so deselect the accept button and move the
                // navigation to the cancel button.
                acceptButton.IsSelected = false;
                cancelButton.IsSelected = true;
            }

            // For this action, we'll play the sound effect at the end of the
            // action so that if the sound effect volume slider was adjusted
            // the sound effect played will have those changes.
            Core.Audio.PlaySoundEffect(soundEffect);
        };

        // Wire up the actions to perform when the Confirm action is triggered
        // for the menu.
        _optionsMenu.ConfirmAction = () =>
        {
            // Play the sound effect.
            Core.Audio.PlaySoundEffect(soundEffect);

            if (musicPanel.IsSelected)
            {
                // The music panel is selected and the confirm action was
                // performed, so deselect the music panel and move the
                // navigation to the sound effect panel.
                musicPanel.IsEnabled = musicPanel.IsSelected = false;
                soundEffectPanel.IsEnabled = soundEffectPanel.IsSelected = true;
            }
            else if (soundEffectPanel.IsSelected)
            {
                // The sound effect panel is selected and the confirm action was
                // performed, so deselect the sound effect panel and move the
                // navigation to the accept button.
                soundEffectPanel.IsEnabled = soundEffectPanel.IsSelected = false;
                acceptButton.IsSelected = true;
            }
            else if (acceptButton.IsSelected)
            {
                // The accept button is selected and the confirm action was
                // performed, so disable the options menu...
                _optionsMenu.IsEnabled = _optionsMenu.IsVisible = _optionsMenu.IsSelected = false;

                // ...and restore the default selections of the options menu in
                // case they come back to it...
                musicPanel.IsEnabled = musicPanel.IsSelected = true;
                soundEffectPanel.IsEnabled = soundEffectPanel.IsSelected = false;
                acceptButton.IsSelected = cancelButton.IsSelected = false;

                // ...and enable the title menu.
                _titleMenu.IsEnabled = _titleMenu.IsVisible = _titleMenu.IsSelected = true;
            }
            else if (cancelButton.IsSelected)
            {
                // The cancel button is selected and the confirm action was
                // performed, so disable the options menu...
                _optionsMenu.IsEnabled = _optionsMenu.IsVisible = _optionsMenu.IsSelected = false;

                // ...and restore the song and sound effect volume levels...
                musicVolumeSlider.Value = Core.Audio.SongVolume = _previousSongVolume;
                soundEffectVolumeSlider.Value = Core.Audio.SoundEffectVolume = _previousSoundEffectVolume;

                // ...and restore the default selections of the options menu in
                // case they come back to it...
                musicPanel.IsEnabled = musicPanel.IsSelected = true;
                soundEffectPanel.IsEnabled = soundEffectPanel.IsSelected = false;
                acceptButton.IsSelected = cancelButton.IsSelected = false;

                // ...and enable the title menu.
                _titleMenu.IsEnabled = _titleMenu.IsVisible = _titleMenu.IsSelected = true;
            }
        };

        // Wire up the actions to perform when the Cancel action is triggered
        // for the menu.
        _optionsMenu.CancelAction = () =>
        {
            // Play the sound effect
            Core.Audio.PlaySoundEffect(soundEffect);

            if (soundEffectPanel.IsSelected)
            {
                // The sound effect panel is selected and the cancel action was
                // performed, so deselect the sound effect panel and move the
                // navigation back to the music panel.
                soundEffectPanel.IsEnabled = soundEffectPanel.IsSelected = false;
                musicPanel.IsEnabled = musicPanel.IsSelected = true;
            }
            else if (acceptButton.IsSelected)
            {
                // The accept button is selected and the cancel action was
                // performed, so deselect the accept button and move the
                // navigation back to the sound effect panel.
                acceptButton.IsSelected = false;
                soundEffectPanel.IsEnabled = soundEffectPanel.IsSelected = true;
            }
            else if (cancelButton.IsSelected)
            {
                // The cancel button is selected and the cancel action was
                // performed, so deselect the cancel button and move the
                // navigation back to the sound effect panel.
                cancelButton.IsSelected = false;
                soundEffectPanel.IsEnabled = soundEffectPanel.IsSelected = true;
            }
        };
    }

    public override void Update(GameTime gameTime)
    {
        // Update the offsets for the background pattern wrapping so that it
        // scrolls down and to the right.
        float offset = _scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        _backgroundOffset.X -= offset;
        _backgroundOffset.Y -= offset;

        // Ensure that the offsets do not go beyond the texture bounds so it is
        // a seamless wrap
        _backgroundOffset.X %= _backgroundPattern.Width;
        _backgroundOffset.Y %= _backgroundPattern.Height;

        _titleMenu.Update(gameTime);
        _optionsMenu.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(new Color(32, 40, 78, 255));

         // Draw the background pattern first using the PointWrap sampler state.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointWrap);
        Core.SpriteBatch.Draw(_backgroundPattern, _backgroundDestination, new Rectangle(_backgroundOffset.ToPoint(), _backgroundDestination.Size), Color.White * 0.5f);
        Core.SpriteBatch.End();

        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _titleMenu.Draw(Core.SpriteBatch);
        _optionsMenu.Draw(Core.SpriteBatch);
        Core.SpriteBatch.End();
    }
}