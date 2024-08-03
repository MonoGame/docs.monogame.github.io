---
title: How to play a Sound(effect)
description: This topic demonstrates how to play a simple sound by using SoundEffect.
requireMSLicense: true
---

## Overview

In this example we will take our first steps into audio nirvana and play a simple one-time-only [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect).

## Requirements

This sample uses a sound file named [tx0_fire1.wav](./files/tx0_fire1.wav), which you can download from [this link here](./files/tx0_fire1.wav). (save-as)
But you can use your own if you wish.

## To add a sound file to your project

1. Open your MonoGame game in your editor of choice.

2. Open the `Content.mgcb` file by either double-clicking on it, or right-clicking and selecting `Open in MGCB editor". (depending on your editor)

3. Click **Add** in the menu bar, and then click **Existing Item**. (alternatively, use the icon for the same task in the editor)

4. Navigate to the sound file you want to add, and then select it.

> The selected audio file is inserted into your project. By default, it is processed by the Content Pipeline, and built wave files are accessed automatically when the game is built.

### To play a simple sound

1. Declare a [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect) object to hold the sound file.

    ```csharp
    // Audio objects
    SoundEffect soundEffect;
    ```

2. Load the sound file using [Content.Load](xref:Microsoft.Xna.Framework.Content.ContentManager) in the 'LoadContent' method.

    ```csharp
    soundEffect = Content.Load<SoundEffect>("tx0_fire1");
    ```

3. Play the sound.

    ```csharp
    // Play the sound
    soundEffect.Play();
    ```

> [!NOTE]
> Obviously you would not normally call `play` in the `LoadContent` method, we just use here as an example.

## Concepts

* [Looping a Sound](HowTo_LoopASound.md)

  Demonstrates how to loop a sound.

## Reference

* [SoundEffect Class](xref:Microsoft.Xna.Framework.Audio.SoundEffect)

  Provides a loaded sound resource.

* [SoundEffectInstance Class](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance)

  Provides a single playing, paused, or stopped instance of a [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect) sound.
