---
title: How to loop Sounds
description: This section demonstrates how to loop a sound.
requireMSLicense: true
---

## Overview

In this example we will go over the basics of how to make sound effects loop rather than the default play once.

## Simple Sound Looping

Not much extra code is needed to continuously loop a sound file in your game. Since the [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect) class does not provide looping support, you will need to allocate a [SoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance) object. The following procedure builds on the sample code provided in the [Playing a Sound](HowTo_PlayASound.md) topic.

### To loop a sound

1. Follow the instructions show in [Playing a Sound](HowTo_PlayASound.md) topic.

2. Declare a [SoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance) object to hold an instance of the sound file.

    ```csharp
    private SoundEffectInstance soundEffectInstance;
    ```

3. To be able to loop a sound you will need to declare a [SoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance) object, and set it to the return value of [SoundEffect.CreateInstance](xref:Microsoft.Xna.Framework.Audio.SoundEffect.CreateInstance). add this after loading the sound file in the `LoadContent` method.

    ```csharp
    soundEffectInstance = soundEffect.CreateInstance();
    ```

4. Set [SoundEffectInstance.IsLooped](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance#Microsoft_Xna_Framework_Audio_SoundEffectInstance_IsLooped) to **true** which will cause the sound effect to play forever or until you stop it or close the game.

    ```csharp
    soundEffectInstance.IsLooped = true;
    ```

5. Play the sound instance (removing the old soundEffect.Play() call).

    ```csharp
    // Play the sound effect instance
    soundEffectInstance.Play();
    ```

> [!NOTE]
> Obviously you would not normally call `play` in the `LoadContent` method, we just use here as an example.

## Concepts

* [Playing a Sound](HowTo_PlayASound.md)

  Demonstrates how to play a simple sound by using [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect).

## Reference

* [SoundEffect Class](xref:Microsoft.Xna.Framework.Audio.SoundEffect)

  Provides a loaded sound resource.

* [SoundEffectInstance Class](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance)

  Provides a single playing, paused, or stopped instance of a [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect) sound.
