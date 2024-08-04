---
title: How to stream Data from a WAV File
description: This topic describes how to stream an audio file using DynamicSoundEffectInstance.
requireMSLicense: true
---

## Overview

In this example we will demonstrate the uses of the [DynamicSoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.DynamicSoundEffectInstance) which is an advanced method for strictly controlling what audio data is going to be played.

One approach is to tightly control the memory being used for playing audio, rather than loading an entire track into memory, you only need to load a sufficient amount to play audio for the current frame or frames.  Very useful when every bit of memory data counts.

Another approach (Which was used in the [Nokia AlphaLabs Experiment - Vequencer](https://martincaine.com/xna/introducing_vequencer_an_alphalabscc_experiment)) enables you to dynamically compose audio at runtime, especially useful when you want to control pacing and the number of instruments playing.  But buyer beware, it can be tricky to get this right. (but awesome when it works)

## Requirements

This sample uses a sound file named [rock_loop_mono.wav](https://github.com/MonoGame/MonoGame/raw/develop/Tests/Assets/Audio/rock_loop_mono.wav), which you can download from [this link here](https://github.com/MonoGame/MonoGame/raw/develop/Tests/Assets/Audio/rock_loop_mono.wav). (save-as)
But you can use your own if you wish.

## Opening a wave file for streaming

1. Create global variables to hold the [DynamicSoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.DynamicSoundEffectInstance), position, count, and data.

    ```csharp
    DynamicSoundEffectInstance dynamicSound;
    int position;
    int count;
    byte[] byteArray;
    ```

2. In the [Game.LoadContent](xref:Microsoft.Xna.Framework.Game.LoadContent) method of your game, open the audio data using [TitleContainer.OpenStream](xref:Microsoft.Xna.Framework.TitleContainer).

    ```csharp
    System.IO.Stream waveFileStream = TitleContainer.OpenStream(@"Content\rock_loop_mono.wav");
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

7. Store the size of the audio buffer that is needed in the count variable, etting data in 100 ms chunks.

    ```csharp
    count = dynamicSound.GetSampleSizeInBytes(TimeSpan.FromMilliseconds(100));
    ```

8. Set up the dynamic sound effect's "buffer needed" event handler, so audio data is only read when it needs more data.

    ```csharp
    dynamicSound.BufferNeeded += new EventHandler<EventArgs>(DynamicSound_BufferNeeded);
    ```

9. Implement the buffer needed event handler after the `LoadContent` method.

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

10. Below the **[Game.Draw](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Draw_Microsoft_Xna_Framework_GameTime_)** method, create a new method called **IsKeyPressed**, which will check if a specified key is pressed and return a boolean value of true if it has been pressed.

    ```csharp
    private bool IsKeyPressed(Keys key)
    {
        return Keyboard.GetState().IsKeyDown(key);
    }
     ```

11. Use the controller buttons to play and stop the sound stream in the `Update` method.

    ```csharp
        // Pressing the spacebar will start the DynamicSoundEffectInstance playing
        if (IsKeyPressed(Keys.Space))
        {
            dynamicSound.Play();
        }

        // Pressing "P" will stop the audio playing.
        if (IsKeyPressed(Keys.P))
        {
            dynamicSound.Pause();
        }
    ```

## See Also

* [Playing a Sound](HowTo_PlayASound.md)

  Demonstrates how to play a simple sound by using [SoundEffect](xref:Microsoft.Xna.Framework.Audio.SoundEffect).

## Reference

* [DynamicSoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.DynamicSoundEffectInstance)

  Provides properties, methods, and events for play back of the audio buffer.

* [BufferNeeded](xref:Microsoft.Xna.Framework.Audio.DynamicSoundEffectInstance)

  Event that occurs when the number of audio capture buffers awaiting playback is less than or equal to two.

* [OpenStream](xref:Microsoft.Xna.Framework.TitleContainer)

  Returns a stream to an existing file in the default title storage location.
