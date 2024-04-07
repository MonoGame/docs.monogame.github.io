---
title: How to stream Data from a WAV File
description: This topic describes how to stream an audio file using DynamicSoundEffectInstance.
---

# Streaming Data from a WAV File

This topic describes how to stream an audio file using [DynamicSoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.DynamicSoundEffectInstance).

## Loading Audio Content

### To open a wave file for streaming

1. Create global variables to hold the [DynamicSoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.DynamicSoundEffectInstance), position, count, and data.

    ```csharp
    DynamicSoundEffectInstance dynamicSound;
    int position;
    int count;
    byte[] byteArray;
    ```

2. In the [Game.LoadContent](xref:Microsoft.Xna.Framework.Game.LoadContent) method of your game, open the audio data using [TitleContainer.OpenStream](xref:Microsoft.Xna.Framework.TitleContainer).

    ```csharp
    System.IO.Stream waveFileStream = TitleContainer.OpenStream(@"Content\48K16BSLoop.wav");
    ```

3. Create a new binary reader to read from the audio stream.

    ```csharp
    BinaryReader reader = new BinaryReader(waveFileStream);
    ```

4. Read the wave file header from the buffer.

    ```csharp
    int chunkID = reader.ReadInt32();
    int fileSize = reader.ReadInt32();
    int riffType = reader.ReadInt32();
    int fmtID = reader.ReadInt32();
    int fmtSize = reader.ReadInt32();
    int fmtCode = reader.ReadInt16();
    int channels = reader.ReadInt16();
    int sampleRate = reader.ReadInt32();
    int fmtAvgBPS = reader.ReadInt32();
    int fmtBlockAlign = reader.ReadInt16();
    int bitDepth = reader.ReadInt16();
    
    if (fmtSize == 18)
    {
        // Read any extra values
        int fmtExtraSize = reader.ReadInt16();
        reader.ReadBytes(fmtExtraSize);
    }
    
    int dataID = reader.ReadInt32();
    int dataSize = reader.ReadInt32();
    ```

5. Store the audio data of the wave file to a byte array.

    ```csharp
    byteArray = reader.ReadBytes(dataSize);
    ```

6. Create the new dynamic sound effect instance using the sample rate and channel information extracted from the sound file.

     ```csharp
     dynamicSound = new DynamicSoundEffectInstance(sampleRate, (AudioChannels)channels);
     ```

7. Store the size of the audio buffer that is needed in the count variable. Submit data in 100 ms chunks.

    ```csharp
    count = dynamicSound.GetSampleSizeInBytes(TimeSpan.FromMilliseconds(100));
    ```

8. Set up the dynamic sound effect's buffer needed event handler so audio data can be read when it needs more data.

    ```csharp
    dynamicSound.BufferNeeded += new EventHandler<EventArgs>(DynamicSound_BufferNeeded);
    ```

9. Implement the buffer needed event handler.

    ```csharp
    void DynamicSound_BufferNeeded(object sender, EventArgs e)
    {
        dynamicSound.SubmitBuffer(byteArray, position, count / 2);
        dynamicSound.SubmitBuffer(byteArray, position + count / 2, count / 2);
    
        position += count;
        if (position + count > byteArray.Length)
        {
            position = 0;
        }
    }
    ```

10. Use the controller buttons to play and stop the sound stream.

    ```csharp
    protected override void Update(GameTime gameTime)
    {
        GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
        if (lastGamePadState != gamePadState)
        {
            if (gamePadState.Buttons.A == ButtonState.Pressed)
            {
                dynamicSound.Play();
            }
            if (gamePadState.Buttons.B == ButtonState.Pressed)
            {
                dynamicSound.Stop();
            }
            lastGamePadState = gamePadState;
        }

        base.Update(gameTime);
    }
    ```

## Concepts

[Playing a Sound](HowTo_PlayASound.md)

Demonstrates how to play a simple sound by using [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect).

## Reference

[DynamicSoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.DynamicSoundEffectInstance)

Provides properties, methods, and events for play back of the audio buffer.

[BufferNeeded](xref:Microsoft.Xna.Framework.Audio.DynamicSoundEffectInstance)

Event that occurs when the number of audio capture buffers awaiting playback is less than or equal to two.

[OpenStream](xref:Microsoft.Xna.Framework.TitleContainer)

Returns a stream to an existing file in the default title storage location.

---

© 2012 Microsoft Corporation. All rights reserved.

© 2023 The MonoGame Foundation.
