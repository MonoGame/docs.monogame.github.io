---
title: How to play a Song / music
description: Demonstrates how to play a song from a media library.
---

# Playing a Song

Demonstrates how to play a song from a media library.

The [Albums](xref:Microsoft.Xna.Framework.Media.MediaLibrary.Albums) property provides access to the media library, and the [Play](xref:Microsoft.Xna.Framework.Media.MediaPlayer) method plays a song. Consider any current audio playback when using the [Play](xref:Microsoft.Xna.Framework.Media.MediaPlayer) method. If the user currently is playing a different song, the [Stop](xref:Microsoft.Xna.Framework.Media.MediaPlayer) method can be used to stop the current song.

## To play a song from a random album in a user's media library

The following demonstrates how to play a song from a randomly picked album (shuffle).

1. Declare [MediaLibrary](xref:Microsoft.Xna.Framework.Media.MediaLibrary) and [Random](http://msdn.microsoft.com/en-us/library/system.random.aspx).

    ```csharp
    MediaLibrary sampleMediaLibrary;
    Random rand;
    ```

2. Initialize [MediaLibrary](xref:Microsoft.Xna.Framework.Media.MediaLibrary) and the random number generator in the game's constructor.

    ```csharp
    sampleMediaLibrary = new MediaLibrary();
    rand = new Random();
    ```

3. Stop the current audio playback by calling [MediaPlayer.Stop](xref:Microsoft.Xna.Framework.Media.MediaPlayer), and then generate a random number that will serve as a valid album index. Next, play the first track in the random album.

    ```csharp
    MediaPlayer.Stop(); // stop current audio playback 
    
    // generate a random valid index into Albums
    int i = rand.Next(0, sampleMediaLibrary.Albums.Count - 1);
    
    // play the first track from the album
    MediaPlayer.Play(sampleMediaLibrary.Albums[i].Songs[0]);
    ```

## Concepts

[Playing a Song from a URI](HowTo_PlaySongfromURI.md)

Demonstrates how to use the [MediaPlayer](xref:Microsoft.Xna.Framework.Media.MediaPlayer) to play a song from a Uniform Resource Identifier (URI).

[Media Overview](../../whatis/WhatIs_Audio.md)

Provides a high-level overview about the capabilities—such as playing music and video and accessing pictures—of the Media API in MonoGame.

## Reference

[MediaPlayer Class](xref:Microsoft.Xna.Framework.Media.MediaPlayer)

Provides methods and properties to play, pause, resume, and stop songs. **MediaPlayer** also exposes shuffle, repeat, volume, play position, and visualization capabilities.

[MediaLibrary Class](xref:Microsoft.Xna.Framework.Media.MediaLibrary)

Provides access to songs, playlists, and pictures in the device's media library.

[MediaPlayer.Play Method](xref:Microsoft.Xna.Framework.Media.MediaPlayer)

Plays a song or collection of songs.

[MediaLibrary.Albums Property](xref:Microsoft.Xna.Framework.Media.MediaLibrary.Albums)

Gets the [AlbumCollection](xref:Microsoft.Xna.Framework.Media.AlbumCollection) that contains all albums in the media library.

---

© 2012 Microsoft Corporation. All rights reserved.

© 2023 The MonoGame Foundation.
