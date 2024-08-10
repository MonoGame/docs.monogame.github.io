---
title: How to play a Song / music
description: Demonstrates how to play a song from a media library.
requireMSLicense: true
---

## Overview

In this example we will demonstrate the ability to load a media file such as an MP3, OGG or WMA file.

The [MediaPlayer](xref:Microsoft.Xna.Framework.Media.MediaPlayer) class manages long running music or media. Will start audio playback when using the [Play](xref:Microsoft.Xna.Framework.Media.MediaPlayer#Microsoft_Xna_Framework_Media_MediaPlayer_Play_Microsoft_Xna_Framework_Media_Song_) method. If the user currently is playing a different song, the [Stop](xref:Microsoft.Xna.Framework.Media.MediaPlayer#Microsoft_Xna_Framework_Media_MediaPlayer_Stop) method can be used to stop the current song.

> [!NOTE]
> Only one song can be played at any time using the MediaPlayer.

## Requirements

This sample uses a mp3 file named [rock_loop_stereo.mp3](https://github.com/MonoGame/MonoGame/raw/develop/Tests/Assets/Audio/rock_loop_stereo.mp3), which you can download from [this link here](https://github.com/MonoGame/MonoGame/raw/develop/Tests/Assets/Audio/rock_loop_stereo.mp3). (save-as)
But you can use your own if you wish.

## To add a music file to your project

1. Open your MonoGame game in your editor of choice.

2. Open the `Content.mgcb` file by either double-clicking on it, or right-clicking and selecting `Open in MGCB editor". (depending on your editor)

3. Click **Add** in the menu bar, and then click **Existing Item**. (alternatively, use the icon for the same task in the editor)

4. Navigate to the music (mp3) file you want to add, and then select it.

> The selected audio file is inserted into your project. By default, it is processed by the Content Pipeline as a [Song](xref:Microsoft.Xna.Framework.Media.Song)

## To play a song

The following demonstrates how to play a song from the media file in the content project.

1. Declare a [Song](xref:Microsoft.Xna.Framework.Media.Song) parameter as a reference for the media file to load.

    ```csharp
    private Song song;
    ```

2. Load the media file using [Content.Load](xref:Microsoft.Xna.Framework.Content.ContentManager) in the 'LoadContent' method.

    ```csharp
    song = Content.Load<Song>("rock_loop_stereo");
    ```

3. Check if the [MediaPlayer](xref:Microsoft.Xna.Framework.Media.MediaPlayer) is currently playing (or has a paused track), and then call "Stop" if needed. As soon as the [MediaPlayer](xref:Microsoft.Xna.Framework.Media.MediaPlayer) is ready, play the music.

    ```csharp
    // check the current state of the MediaPlayer.
    if(MediaPlayer.State != MediaState.Stopped)
    {
        MediaPlayer.Stop(); // stop current audio playback if playing or paused.
    }

    // Play the selected song reference.
    MediaPlayer.Play(song);
    ```

## See Also

* [Media Overview](../../whatis/audio/index.md)

  Provides a high-level overview about the capabilities—such as playing music and video and accessing pictures—of the Media API in MonoGame.

## Reference

* [MediaPlayer Class](xref:Microsoft.Xna.Framework.Media.MediaPlayer)

  Provides methods and properties to play, pause, resume, and stop songs. **MediaPlayer** also exposes shuffle, repeat, volume, play position, and visualization capabilities.
