---
title: How to play a Sound(effect)
description: This topic demonstrates how to play a simple sound by using SoundEffect.
---

# Playing a Sound

This topic demonstrates how to play a simple sound by using [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect).

## To add a sound file to your project

1. Load your MonoGame game in Visual Studio.

2. In **Solution Explorer**, right-click the solution's **Content** project.

3. Click **Add**, and then click **Existing Item**.

4. Navigate to the .wav file you want to add, and then select it.

> The selected .wav file is inserted into your project. By default, it is processed by the Content Pipeline, and built wave files are accessed automatically when the game is built.

### To play a simple sound

1. Declare a [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect) object to hold the sound file.

    ```csharp
    // Audio objects
    SoundEffect soundEffect;
    ```

2. Load the sound file using [Content.Load](xref:Microsoft.Xna.Framework.Content.ContentManager).

    ```csharp
    soundEffect = Content.Load<SoundEffect>("kaboom");
    ```

3. Play the sound.

    ```csharp
    // Play the sound
    soundEffect.Play();
    ```

## Concepts

[Looping a Sound](HowTo_LoopASound.md)

Demonstrates how to loop a sound.

## Reference

[SoundEffect Class](xref:Microsoft.Xna.Framework.Audio.SoundEffect)

Provides a loaded sound resource.

[SoundEffectInstance Class](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance)

Provides a single playing, paused, or stopped instance of a [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect) sound.

---

© 2012 Microsoft Corporation. All rights reserved.

© 2023 The MonoGame Foundation.
