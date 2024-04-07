---
title: Sounds Overview
description: An overview of how the MonoGame Framework provides audio playback through several core audio classes.
---

# Introduction

If your game is to use a few sound files, then the [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect), [SoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance), and [DynamicSoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.DynamicSoundEffectInstance) classes will provide everything you need to play and stream audio during gameplay.

## Simple Audio Playback

The simplest way to play sounds for background music or sound effects is to use [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect) and [SoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance). Source audio files are added like any other game asset to the project. For example code, see [Playing a Sound](../HowTo/audio/HowTo_PlayASound.md), [Looping a Sound](../HowTo/audio/HowTo_LoopASound.md), and [Adjusting Pitch and Volume](../HowTo/audio/HowTo_ChangePitchAndVolume.md). For background music, see [Playing a Song](../HowTo/audio/HowTo_PlayASong.md).

## Accessing the Audio Buffer

Developers can use [DynamicSoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.DynamicSoundEffectInstance) for direct access to an audio buffer. By accessing the audio buffer, developers can manipulate sound, break up large sound files into smaller data chunks, and stream sound. For example code, see [Streaming Data from a WAV File](../HowTo/audio/HowTo_StreamDataFromWav.md).

## 3D Audio

The [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect) class provides the ability to place audio in a 3D space. By creating [AudioEmitter](xref:Microsoft.Xna.Framework.Audio.AudioEmitter) and [AudioListener](xref:Microsoft.Xna.Framework.Audio.AudioListener) objects, the API can position a sound in 3D, and can change the 3D position of a sound during playback. Once you create and initialize [AudioEmitter](xref:Microsoft.Xna.Framework.Audio.AudioEmitter) and [AudioListener](xref:Microsoft.Xna.Framework.Audio.AudioListener), call [SoundEffectInstance.Apply3D](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance).

## Audio Constraints

When working with multiple platforms using MonoGame, there are a few constraints around audio that you will need to keep in mind and cater for, namely:

* Mobile platforms have a maximum of 32 sounds playing simultaneously.
* Desktop platforms have a maximum of 256 sounds playing simultaneously.
* Consoles and other platforms have their own constraints, please look at the console sdk documentation for more information,

An [InstancePlayLimitException](xref:Microsoft.Xna.Framework.Audio.InstancePlayLimitException) exception is thrown if this limit is exceeded.

## Audio Buffer Format

The byte\[\] buffer format used as a parameter for the [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect) constructor, [Microphone.GetData](xref:Microsoft.Xna.Framework.Audio.Microphone) method, and [DynamicSoundEffectInstance.SubmitBuffer](xref:Microsoft.Xna.Framework.Audio.DynamicSoundEffectInstance) method is PCM wave data. Additionally, the PCM format is interleaved and in little-endian.

The audio format has the following constraints:

* The audio channels can be mono (1) or stereo (2).
* The PCM wave file must have 16-bits per sample.
* The sample rate must be between 8,000 Hz and 48,000 Hz.
* The interleaving for stereo data is left channel to right channel.

## Songs as Background Music

Access to the media library, combined with the ability to use playlists, allows games to create interesting background scores that can change with gameplay. Songs can be played directly from the media library, or can be imported by using the Content Pipeline. For more information, see [Playing a Song](../howto/audio/HowTo_PlayASong.md).

## Concepts

[Playing a Sound](../howto/audio/HowTo_PlayASound.md)

Demonstrates how to play a simple sound by using [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect).

[Streaming Data from a WAV File](../howto/audio/HowTo_StreamDataFromWav.md)

Demonstrates how to stream audio from a wave (.wav) file.

[Playing a Song](../howto/audio/HowTo_PlayASong.md)

Demonstrates how to play a song from a user's media library.

[Playing a Song from a URI](../howto/audio/HowTo_PlaySongfromURI.md)

Demonstrates how to use the [MediaPlayer](xref:Microsoft.Xna.Framework.Media.MediaPlayer) to play a song from a Uniform Resource Identifier (URI).

## Reference

[SoundEffect Class](xref:Microsoft.Xna.Framework.Audio.SoundEffect)

Provides a loaded sound resource.

[SoundEffectInstance Class](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance)

Provides a single playing, paused, or stopped instance of a [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect) sound.

[DynamicSoundEffectInstance Class](xref:Microsoft.Xna.Framework.Audio.DynamicSoundEffectInstance)

Provides properties, methods, and events for play back of the audio buffer.

[Song Class](xref:Microsoft.Xna.Framework.Media.Song)

Provides access to a song in the song library.

---

© 2012 Microsoft Corporation. All rights reserved.

© 2023 The MonoGame Foundation.
