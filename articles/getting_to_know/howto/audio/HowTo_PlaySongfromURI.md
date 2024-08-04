---
title: How to play a Song from a URI
description: Demonstrates how to use the MediaPlayer to play a song from a Uniform Resource Identifier (URI).
requireMSLicense: true
---

## Overview

In this example we will demonstrate streaming a media file using a web URI address (url)

Demonstrates how to use the [MediaPlayer](xref:Microsoft.Xna.Framework.Media.MediaPlayer) to play a song from a Uniform Resource Identifier (URI).

## To play a song from a URI

The following steps show you how to play an .mp3 song located on the Internet.

1. Set [MediaPlayer](xref:Microsoft.Xna.Framework.Media.MediaPlayer) play state to stopped.

    ```csharp
    MediaPlayer.Stop();
    ```

2. Declare a new instance of [Uri](http://msdn.microsoft.com/en-us/library/system.uri.aspx) using the full URI path to the song.

    ```csharp
    Uri uriStreaming = new Uri("https://github.com/MonoGame/MonoGame/raw/develop/Tests/Assets/Audio/rock_loop_stereo.mp3");
    ```

3. Use the [FromUri](xref:Microsoft.Xna.Framework.Media.Song) method to supply an arbitrary string name for the URI, and then pass the [Uri](http://msdn.microsoft.com/en-us/library/system.uri.aspx) object you created in the previous step.

    ```csharp
    Song song = Song.FromUri("StreamingUri", uriStreaming);
    ```

4. Play the song.

    ```csharp
    MediaPlayer.Play(song);
    ```

## See Also

[Playing a Song](HowTo_PlayASong.md)

Demonstrates how to play a song from a user's media library.

[Media Overview](../../whatis/WhatIs_Audio.md)

Provides a high-level overview about the capabilities—such as playing music and video and accessing pictures—of the Media API in XNA Game Studio.

## Reference

[MediaPlayer Class](xref:Microsoft.Xna.Framework.Media.MediaPlayer)

Provides methods and properties to play, pause, resume, and stop songs. **MediaPlayer** also exposes shuffle, repeat, volume, play position, and visualization capabilities.

[Song Class](xref:Microsoft.Xna.Framework.Media.Song)

Provides access to a song in the song library.
