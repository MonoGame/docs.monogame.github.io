---
title: How to loop Sounds
description: This section demonstrates how to loop a sound.
---

# Looping a Sound

This section demonstrates how to loop a sound.

## Simple Sound Looping

Not much extra code is needed to continuously loop a sound file in your game. Since the [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect) class does not provide looping support, you'll need to allocate a [SoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance) object. The following procedure builds on the sample code provided in the [Playing a Sound](HowTo_PlayASound.md) topic.

### To loop a sound

1. Follow the instructions show in [Playing a Sound](HowTo_PlayASound.md) topic.

2. To be able to loop a sound you will need to declare a [SoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance) object, and set it to the return value of [SoundEffect.CreateInstance](xref:Microsoft.Xna.Framework.Audio.SoundEffect.CreateInstance).

    ```csharp
    SoundEffectInstance instance = soundEffect.CreateInstance();
    ```

3. Set [SoundEffectInstance.IsLooped](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance.IsLooped) to **true** and then play the sound.

    ```csharp
    instance.IsLooped = true;
    ```

## Concepts

[Playing a Sound](HowTo_PlayASound.md)

Demonstrates how to play a simple sound by using [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect).

## Reference

[SoundEffect Class](xref:Microsoft.Xna.Framework.Audio.SoundEffect)

Provides a loaded sound resource.

[SoundEffectInstance Class](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance)

Provides a single playing, paused, or stopped instance of a [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect) sound.

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
