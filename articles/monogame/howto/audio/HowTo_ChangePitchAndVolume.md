---
title: How to adjust Pitch and Volume
description: Demonstrates how to manipulate the pitch and volume of sound effects as they play.
---

# Adjusting Pitch and Volume

The [SoundEffect.Play](xref:Microsoft.Xna.Framework.Audio.SoundEffect.Play) method allows you to specify the pitch and volume of a sound to play. However, after you call [Play](xref:Microsoft.Xna.Framework.Audio.SoundEffect.Play), you cannot modify the sound. Using [SoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance) for a given [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect) allows you to change the pitch and volume of a sound at any time during playback.

## Change Pitch and Volume of Sound

1. Declare [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect) and [Stream](http://msdn.microsoft.com/en-us/library/system.io.stream.aspx) by using the method shown in [Playing a Sound](HowTo_PlayASound.md). In addition to the method described in [Playing a Sound](HowTo_PlayASound.md), declare [SoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance).

    ```csharp
    SoundEffectInstance soundInstance;
    ```

2. In the [Game.LoadContent](xref:Microsoft.Xna.Framework.Game.LoadContent) method, set the SoundEffectInstance object to the return value of [SoundEffect.CreateInstance](xref:Microsoft.Xna.Framework.Audio.SoundEffect.CreateInstance).

    ```csharp
    soundfile = TitleContainer.OpenStream(@"Content\tx0_fire1.wav");
    soundEffect = SoundEffect.FromStream(soundfile);
    soundInstance = soundEffect.CreateInstance();
    ```

3. Adjust the sound to the desired level using the [SoundEffectInstance.Pitch](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance.Pitch) and [SoundEffectInstance.Volume](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance.Volume) properties.

    ```csharp
    // Play Sound
    soundInstance.Play();
    ```

4. Play the sound using [SoundEffectInstance.Play](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance.Play).

    ```csharp
    private bool IsKeyPressed(Keys key)
    {
        return Keyboard.GetState().IsKeyDown(key);
    }
	```

 6. In the **[Game.Update](xref:Microsoft.Xna.Framework.Game.Update)** method, check if the **Space** key is pressed and adjust the pitch and volume of the sound effect accordingly. The pitch and volume values are adjusted by +0.1f each time the **Space key** is pressed. The pitch values are clamped to a minimum value of -1.0f and a maximum value of 1.0f, and the volume values are then clamped to a minimum value of 0f and a maximum value of 1.0f. This is done to ensure that the pitch and volume values are within  valid ranges.

	```csharp
    // Check if the SpaceKey is pressed and play the instance
    if (IsKeyPressed(Keys.Space))
        {
         pitch += 0.1f;
         volume += 0.1f;
         pitch = MathHelper.Clamp(pitch, -1.0f, 1.0f);
         volume = MathHelper.Clamp(volume, 0f, 1.0f);
         soundEffectInstance.Pitch = pitch;
         soundEffectInstance.Volume = volume;
         soundEffectInstance.Play();
        }
	```

 > [!NOTE]
 > The **MathHelper.Clamp** method is used to ensure that the pitch and volume values are within the valid range. The pitch value is clamped between -1 and 1, while the volume value is clamped between 0 and 1.

 > [!NOTE]
 > The check for the keypress does not prevent the call to the method repeating so any value entered may peak the value in a singe key press. To prevent this, you can add a delay to the key press check, or use a boolean value to check if the key has been pressed and released.

## Concepts

[Playing a Sound](HowTo_PlayASound.md)

Demonstrates how to play a simple sound by using [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect).

[Looping a Sound](HowTo_LoopASound.md)

Demonstrates how to loop a sound.

[Creating and Playing Sounds](../../whatis/WhatIs_Audio.md)

Provides overviews about audio technology, and presents predefined scenarios to demonstrate how to use audio.

## Reference

[SoundEffect Class](xref:Microsoft.Xna.Framework.Audio.SoundEffect)

Provides a loaded sound resource.

[SoundEffectInstance Class](xref:Microsoft.Xna.Framework.Audio.SoundEffectInstance)

Provides a single playing, paused, or stopped instance of a [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect) sound.

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2024 The MonoGame Foundation.
