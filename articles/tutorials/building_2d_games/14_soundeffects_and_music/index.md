---
title: "Chapter 14: SoundEffects and Music"
description: "Learn how to load and play sound effects and background music in MonoGame including managing audio volume, looping, and handling multiple sound effects at once."
---

In [Chapter 13](../13_collision_detection/index.md), we implemented collision detection to enable interactions between game objects; the slime can now "eat" the bat, which respawns in a random location, while the bat bounces off screen edges. While these mechanics work visually, our game lacks an important element of player feedback: audio.

Audio plays a crucial role in game development by providing immediate feedback for player actions and creating atmosphere. Sound effects alert players when events occur (like collisions or collecting items), while background music helps establish mood and atmosphere.

In this chapter, you will:

- Learn how MonoGame handles different types of audio content.
- Learn how to load and play sound effects and music using the content pipeline.
- Implement sound effects for collision events.
- Add background music to enhance atmosphere.
- Control audio playback including volume and looping.

Let's start by understanding how MonoGame approaches audio content.

## Understanding Audio in MonoGame

Recall from [Chapter 01](../01_what_is_monogame/index.md) that MonoGame is an implementation of the XNA API.  With XNA, there were two methods for implementing audio in your game: the *Microsoft Cross-Platform Audio Creation Tool* (XACT) and the simplified sound API.

> [!IMPORTANT]
> XACT is a mini audio engineering studio where you can easily edit the audio for your game like editing volume, pitch, looping, applying effects, and other properties without having to do it in code. At that time, XACT for XNA games was akin to what FMOD Studio is today for game audio.
>
> | ![Figure 14-1: Microsoft Cross-Platform Audio Creation Tool](./images/xact-editor.png) |
> |:--------------------------------------------------------------------------------------:|
> |             **Figure 14-1: Microsoft Cross-Platform Audio Creation Tool**              |
>
> While XACT projects are still fully supported in MonoGame, it remains a Windows-only tool that has not been updated since Microsoft discontinued the original XNA, nor has its source code been made open source. Though it is possible to install XACT on modern Windows, the process can be complex.
>
> For these reasons, this tutorial will focus on the simplified sound API, which provides all the core functionality needed for most games while remaining cross-platform compatible.

The simplified sound API approaches audio management through two distinct paths, each optimized for different use cases in games. When adding audio to your game, you need to consider how different types of sounds should be handled:

- **Sound Effects**: Short audio clips that need to play immediately and often simultaneously, like the bounce of a ball or feedback for picking up a collectable.
- **Music**: Longer audio pieces that play continuously in the background, like level themes.

MonoGame addresses these different needs through two main classes:

### Sound Effects

The [**SoundEffect**](xref:Microsoft.Xna.Framework.Audio.SoundEffect) class handles short audio clips like:

- Collision sounds.
- Player action feedback (jumping, shooting, etc.).
- UI interactions (button clicks, menu navigation).
- Environmental effects (footsteps, ambient sounds).

The key characteristics of sound effects are:

- Loaded entirely into memory for quick access
- Can play multiple instances simultaneously:
  - Mobile platforms can have a maximum of 32 sounds playing simultaneously.
  - Desktop platforms have a maximum of 256 sounds playing simultaneously.
  - Consoles and other platforms have their own constraints, and you would need to refer to the SDK documentation for that platform.
- Lower latency playback (ideal for immediate feedback)
- Individual volume control per instance.

### Music

The [**Song**](xref:Microsoft.Xna.Framework.Media.Song) class handles longer audio pieces like background music.  The key characteristics of songs are:

- Streamed from storage rather than loaded into memory.
- Only one song can be played at a time.
- Higher latency, but lower memory usage.

Throughout this chapter, we will use both classes to add audio feedback to our game; sound effects for the bat bouncing and being eaten by the slime, and background music to create atmosphere.

## Loading Audio Content

Just like textures, audio content in MonoGame can be loaded through the content pipeline, optimizing the format for your target platform.

### Supported Audio Formats

MonoGame supports several audio file formats for both sound effects and music:

- `.wav`: Uncompressed audio, ideal for short sound effects
- `.mp3`: Compressed audio, better for music and longer sounds
- `.ogg`: Open source compressed format, supported on all platforms
- `.wma`: Windows Media Audio format (not recommended for cross-platform games)

> [!TIP]
> For sound effects, `.wav` files provide the best loading and playback performance since they do not need to be decompressed. For music, `.mp3` or `.ogg` files are better choices as they reduce file size while maintaining good quality.

### Adding Audio Files

Adding audio files can be done through the content pipeline, just like we did for image files, using the MGCB Editor.  When you add an audio file to the content project, the MGCB Editor will automatically select the appropriate importer and processor for the audio file based on the file extension.

The processor that are available for audio files file:

- **Sound Effects**: Processes the audio file as a [**SoundEffect**](xref:Microsoft.Xna.Framework.Audio.SoundEffect).  This is  automatically selected for *.wav* files.
- **Song**: Processes the audio file as a [**Song**](xref:Microsoft.Xna.Framework.Media.Song).  This is automatically selected for *.mp3*, *.ogg*, and *.wma* files.

| [!Figure 14-2: MGCB Editor properties panel showing Sound Effect content processor settings for .wav files](./images/sound-effect-properties.png) | [!Figure 14-3: MGCB Editor properties panel showing Song content processor settings for .mp3 files](./images/song-properties.png) |
|:-------------------------------------------------------------------------------------------------------------------------------------------------:|:---------------------------------------------------------------------------------------------------------------------------------:|
|                   **Figure 14-2: MGCB Editor properties panel showing Sound Effect content processor settings for .wav files**                    |                **Figure 14-3: MGCB Editor properties panel showing Song content processor settings for .mp3 files                 |

> [!NOTE]
> While you typically will not need to change the processor it automatically selects, there may be times where you add files, such as *.mp3* files that are meant to be sound effects and not songs.  Always double check that the processor selected is for the intended type.

### Loading Sound Effects

To load a sound effect, we use [**ContentManager.Load**](xref:Microsoft.Xna.Framework.Content.ContentManager.Load``1(System.String)) with the [**SoundEffect**](xref:Microsoft.Xna.Framework.Audio.SoundEffect) type:

[!code-csharp[](./snippets/load_soundeffect.cs)]

### Loading Music

Loading music is similar, only we specify the [**Song**](xref:Microsoft.Xna.Framework.Media.Song) type instead.

[!code-csharp[](./snippets/load_song.cs)]

## Playing Sound Effects

Sound effects are played using the [**SoundEffect**](xref:Microsoft.Xna.Framework.Audio.SoundEffect) class. This class provides two ways to play sounds:

1. Direct playback using [**SoundEffect.Play**](xref:Microsoft.Xna.Framework.Audio.SoundEffect.Play):

    [!code-csharp[](./snippets/play_soundeffect.cs)]

2. Creating an instance using [**SoundEffect.CreateInstance**](xref:Microsoft.Xna.Framework.Audio.SoundEffect.CreateInstance):

    [!code-csharp[](./snippets/play_soundeffect_instance.cs)]

- Use [**SoundEffect.Play**](xref:Microsoft.Xna.Framework.Audio.SoundEffect.Play) for simple sound effects that you just want to play once.
- Use [**SoundEffect.CreateInstance**](xref:Microsoft.Xna.Framework.Audio.SoundEffect.CreateInstance) when you need more control over the sound effect, like adjusting volume, looping, or managing multiple instances of the same sound.  

[**SoundEffectInstance**](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance) contains several properties that can be used to control how the sound effect is played:

| Property                                                                        | Type                                                            | Description                                                                |
|---------------------------------------------------------------------------------|-----------------------------------------------------------------|----------------------------------------------------------------------------|
| [**IsLooped**](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance.IsLooped) | `bool`                                                          | Whether the sound should loop when it reaches the end.                     |
| [**Pan**](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance.Pan)           | `float`                                                         | Stereo panning between -1.0f (full left) and 1.0f (full right).            |
| [**Pitch**](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance.Pitch)       | `float`                                                         | Pitch adjustment between -1.0f (down one octave) and 1.0f (up one octave). |
| [**State**](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance.State)       | [**SoundState**](xref:Microsoft.Xna.Framework.Audio.SoundState) | Current playback state (Playing, Paused, or Stopped).                      |
| [**Volume**](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance.Volume)     | `float`                                                         | Volume level between 0.0f (silent) and 1.0f (full volume).                 |

## Playing Music

Unlike sound effects, music is played through the [**MediaPlayer**](xref:Microsoft.Xna.Framework.Media.MediaPlayer) class. This static class manages playback of [**Song**](xref:Microsoft.Xna.Framework.Media.Song) instances and provides global control over music playback:

[!code-csharp[](./snippets/play_song.cs)]

> [!IMPORTANT]
> While [**SoundEffect**](xref:Microsoft.Xna.Framework.Audio.SoundEffect) instances can be played simultaneously, trying to play a new [**Song**](xref:Microsoft.Xna.Framework.Media.Song) while another is playing will stop the current song in the best case, and in the worst case cause a crash on some platforms.  In the example above, the state of the media player is checked first before we tell it to play a song.  Checking the state first and stopping it manually if it is playing is best practice to prevent potential crashes.

## Audio Management

While playing sounds and music is straightforward, a complete game needs to handle various audio states and cleanup. An audio manager that will:

- Track and manage sound effects and songs
- Handle volume control
- Manage audio state (pause/resume, mute/unmute)
- Clean up resources properly

To get started, create a new directory called *Audio* in the *MonoGameLibrary* project.

### The AudioManager Class

To effectively manage audio in our games, we will create an `AudioManager` class that handles loading, playing, and controlling both sound effects and music. This manager will be implemented as a [**GameComponent**](xref:Microsoft.Xna.Framework.GameComponent), allowing it to receive automatic updates and cleanup.

In the *Audio* directory of the *MonoGameLibrary* project, add a new file named *AudioManager.cs* with this initial structure:

[!code-csharp[](./snippets/audiomanager.cs#declaration)]

> [!TIP]
> The `AudioManager` class inherits from [**GameComponent**](xref:Microsoft.Xna.Framework.GameComponent), which allows it to be added to a game's component collection and automatically receive updates.

#### AudioManager Members

The `AudioManager` class needs to track various audio resources and states. Add these private fields:

[!code-csharp[](./snippets/audiomanager.cs#fields)]

These fields serve different purposes:

- `_soundEffects`: Stores loaded sound effects by their asset name.
- `_songs`: Stores loaded songs by their asset name.
- `_activeSoundEffectInstances`: Tracks currently playing sound effects.
- `_previousMusicVolume` and `_previousSoundEffectVolume`: Store volume levels for mute/unmute functionality.

### AudioManager Properties

The AudioManager provides a property to track its mute state.  Add the following property:

[!code-csharp[](./snippets/audiomanager.cs#properties)]

### AudioManager Constructor

The constructor initializes our collections and sets up the component.  Add the following constructor:

[!code-csharp[](./snippets/audiomanager.cs#ctors)]

### AudioManager Methods

The `AudioManager` class provides several categories of methods to handle different aspects of audio management.  Each group of methods serves a specific purpose in managing game audio, from basic playback to more complex state management.

#### Game Component Methods

Add the following methods to the manager which override key methods from [**GameComponent**](xref:Microsoft.Xna.Framework.GameComponent):

[!code-csharp[](./snippets/audiomanager.cs#methods_gamecomponent)]

- `Initialize`: Sets up initial audio states by setting the default volume levels for both music and sound effects to full volume (1.0f). These values are also stored as the previous volumes for use when unmuting.
- `Update`: Handles cleanup of completed sound effects. Each frame, it checks for any sound effect instances that have finished playing (reached the Stopped state) and disposes of them to free up resources.
- `Dispose`: Ensures proper cleanup of audio resources when the game closes. All sound effects and songs are disposed of, and the collections tracking them are cleared.

#### Content Management Methods

Add the following methods to handle handle loading audio content into the manager:

[!code-csharp[](./snippets/audiomanager.cs#methods_content)]

- `AddSoundEffect`: Loads a sound effect from the content pipeline using the provided asset name and stores it in the `_soundEffects` dictionary for later use.
- `AddSong`: Similar to `AddSoundEffect`, this loads a song from the content pipeline and stores it in the `_songs` dictionary.

#### Playback Methods

Add the following methods to control audio playback:

[!code-csharp[](./snippets/audiomanager.cs#methods_playback)]

- `PlaySoundEffect`: Two overloads of this method are implemented.  The first can be used to quickly fire off a sound effect if you do not need to adjust additional properties.  The second contains parameters to customize the volume, pitch, panning, and looping properties of the sound effect.  Both methods returns the instance for further control if needed.
- `PlaySong`: Starts playing a song through the MediaPlayer. Since only one song can play at a time, this will automatically stop any currently playing song.

#### State Control Methods

Add the following methods to manage the overall state of audio playback:

[!code-csharp[](./snippets/audiomanager.cs#methods_state)]

- `PauseAudio`: Pauses all currently playing audio including both the active song and any playing sound effects.
- `ResumeAudio`: Resumes playback of previously paused audio, both for the song and sound effects.
- `MuteAudio`: Silences all audio by setting volumes to zero while storing previous volumes.
- `UnmuteAudio`: Restores audio to the volume levels that were active before muting.
- `ToggleMute`: Provides a convenient way to switch between muted and unmuted states.

#### Volume Control Methods

Finally, add the following methods to adjusting the volume of all audio:

[!code-csharp[](./snippets/audiomanager.cs#methods_volume)]

- `IncreaseVolume`: Raises the volume of both music and sound effects by the specified amount, ensuring it does not exceed the maximum (1.0f).
- `DecreaseVolume`: Lowers the volume of both music and sound effects by the specified amount, ensuring it does not go below zero.

## Adding Audio To Our Game

Before we can add audio to our game, we need some sound files to work with. Download the following audio files:

- [bounce.wav](./files/bounce.wav) - For when the bat bounces off screen edges
- [collect.wav](./files/collect.wav) - For when the slime eats the bat
- [theme.mp3](./files/theme.mp3) - Background music

> [!NOTE]
>
> - *bounce.wav* is "Retro Impact Punch 07" by Davit Masia (https://kronbits.itch.io/retrosfx).
> - *collect.wav* is "Retro Jump Classic 08" by Davit Masia (https://kronbits.itch.io/retrosfx).
> - *theme.mp3* is "8bit Dungeon Level" by Kevin MacLeod (incompetech.com), Licensed under Creative Commons: By Attribution 4.0 License http://creativecommons.org/licenses/by/4.0/

Add these files to your content project using the MGCB Editor:

1. Open the *Content.mgcb* file in the MGCB Editor.
2. Create a new directory called `audio` (right-click *Content* > *Add* > *New Folder*).
3. Right-click the new *audio* directory and choose *Add* > *Existing Item...*.
4. Navigate to and select the audio files you downloaded.
5. For each file that is added, check its properties in the Properties panel:
   - For `.wav` files, ensure the *Processor* is set to `Sound Effect`.
   - For `.mp3` files, ensure the *Processor* is set to `Song`.

Next, let's implement the `AudioManager` game component that was created in our game to manage audio. Open the *Game1.cs* and make the following changes:

[!code-csharp[](./snippets/game1.cs?highlight=6,38-39,53-57,73-74,90-97,198-199,222-223,280-296)]

The key changes made here are:

1. The `using MonoGameLibrary.Audio` using directive was added so we can use the new `AudioManager` class.
2. The field `_audio` was added to track and access the `AudioManager`.
3. In the constructor, a new instance of the `AudioManager` is created and added to the game's component collection.
4. In [**LoadContent**](xref:Microsoft.Xna.Framework.Game.LoadContent), the bounce, collect, and theme sound effects and song are added to the audio manager.
5. In [**Initialize**](xref:Microsoft.Xna.Framework.Game.Initialize) the audio manager is told to start play the theme song.
6. In [**Update**](xref:Microsoft.Xna.Framework.Game.Update(Microsoft.Xna.Framework.GameTime)) the following changes were made:
    1. When it is detected that the bat should bounce off the edge of the screen, the bounce sound effect is played
    2. When it is detected that the slime eats the bat, the collect sound effect is played.
7. In `CheckKeyboardInput`, the following changes were made:
    1. When the M key is pressed, the audio mute is toggled on/off.
    2. When the + key is pressed, the audio volume is increased.
    3. WHen the - key is pressed, the audio volume is decreased.

Running the game now, the background music will start playing when the game launches, the bounce sound effect will play each time the bat bounces off the edge of the screen, and the collect sound effect will play each time the slime eats the bat.  In addition, you can use the following controls to control the audio through the audio manager:

- M key to toggle mute/unmute.
- Plus (+) key to increase volume.
- Minus (-) key to decrease volume.

## Conclusion

Let's review what you accomplished in this chapter:

- Learned about MonoGame's audio system including sound effects and music.
- Added sound effects to our game for:
  - Bat bouncing off screen edges.
  - Slime collecting the bat.
- Implemented background music.
- Created a reusable `AudioManager` class that:
  - Manages loading and playback of audio content.
  - Handles volume control and muting.
  - Automatically cleans up audio resources.
  - Integrates with the game component system.

In the next chapter, we will explore scene management to handle different game screens like a title screen and a gameplay screen.

## Test Your Knowledge

1. What are the two main classes MonoGame provides for audio playback and how do they differ?

    :::question-answer
    MonoGame provides [**SoundEffect**](xref:Microsoft.Xna.Framework.Audio.SoundEffect) for short audio clips (loaded entirely into memory, multiple can play at once) and [**Song**](xref:Microsoft.Xna.Framework.Media.Song) for longer audio like music (streamed from storage, only one can play at a time).
    :::

2. What is the advantage of using the content pipeline for loading audio files?

    :::question-answer
    The content pipeline processes audio files into an optimized format for your target platform, manages asset copying to the output directory, and provides a consistent way to load content at runtime through the [**ContentManager**](xref:Microsoft.Xna.Framework.Content.ContentManager).
    :::

3. Why did we create the `AudioManager` as a game component?

    :::question-answer
    By inheriting from [**GameComponent**](xref:Microsoft.Xna.Framework.GameComponent), the `AudioManager` receives automatic updates and cleanup through the game's component system.
    :::
