---
title: How to record Sound with a Microphone
description: This topic demonstrates the basics of recording audio using a microphone.
---

# Recording Sound with a Microphone

This topic demonstrates the basics of recording audio using a microphone.

The procedure shows the bare basics of using the microphone to record audio. In practice, you will want to use UI elements (such as buttons) to start and stop audio recording.

## To record audio from the microphone

1. Get the device's default microphone by using [Microphone.Default](xref:Microsoft.Xna.Framework.Audio.Microphone).

    ```csharp
    Microphone  mic = Microphone.Default;
    if (mic == null)
    {
       return false; // No microphone is attached to the device
    }
    ```

2. Declare a buffer array to hold the audio data, and declare a variable to track the amount of audio that will be recorded.

    ```csharp
    // Declare a buffer array to hold the audio data based on the duration of the recording
    byte[] buffer = new byte[Microphone.Default.GetSampleSizeInBytes(TimeSpan.FromSeconds(3.0))];

    // Tracks the amount of data recorded
    int bytesRead = 0;
    ```

3. Begin recording audio, calling method [Microphone.Start](xref:Microsoft.Xna.Framework.Audio.Microphone).

    ```csharp
    if (!isMicrophoneRecording)
    {
        // we are starting to record
        timeSpentRecording = 0;
        Microphone.Default.Start();
    }

    isMicrophoneRecording = !isMicrophoneRecording;
    ```

4. While the microphone is recording, call [Microphone.GetData (Byte\[\], Int32, Int32)](xref:Microsoft.Xna.Framework.Audio.Microphone) in your game's [Game.Update](xref:Microsoft.Xna.Framework.Game) method to save the latest recorded audio to the buffer.

    ```csharp
    bytesRead += Microphone.Default.GetData(buffer, bytesRead, (buffer.Length - bytesRead));
    ```

5. When you are finished recording, set the microphone state to stopped by calling method [Stop](xref:Microsoft.Xna.Framework.Audio.Microphone).

    ```csharp
    if (!isMicrophoneRecording)
    {
    }

    else
    {

        Microphone.Default.Stop();                
        
        // mark the flag that we have some data available.
        hasData = true;
    }
    ```

## Concepts

[Working with Microphones](HowTo_Microphone.md)

Provides basic information about microphone usage in games for Windows Phone.

## Reference

[Microphone](xref:Microsoft.Xna.Framework.Audio.Microphone)

Provides properties, methods, and fields and events for capturing audio data with microphones.

[DynamicSoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.DynamicSoundEffectInstance)

Provides properties, methods, and events for play back of the audio buffer.

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
