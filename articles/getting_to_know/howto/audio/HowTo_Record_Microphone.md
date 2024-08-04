---
title: How to record Sound with a Microphone
description: This topic demonstrates the basics of recording audio using a microphone.
requireMSLicense: true
---

## Overview

The following procedure shows the bare basics of using the microphone to record audio. In practice, you will want to use UI elements (such as buttons) to start and stop audio recording.

As handling microphone audio can become quite complex, this tutorial is split into two halves:

* Part 1 walks through the required code in order to record audio from the default microphone and then stop recording.
* Part 2 is a more in-depth sample which also processes the recorded audio and plays it back to you.

## Part 1, Guide to recording audio from the microphone

1. Get the device's default microphone by using [Microphone.Default](xref:Microsoft.Xna.Framework.Audio.Microphone).

    ```csharp
    Microphone activeMicrophone;

    activeMicrophone = Microphone.Default;
    if (activeMicrophone != null)
    {

    }
    else
    {
           // No microphone is attached to the device
    }
    ```

2. Set the size (duration) of the Microphone buffer and wire up the event to handle the data as it becomes available.  THen declare a buffer array to hold the audio data from these settings.

    ```csharp
    // Set the capture buffer size for low latency.
    // Microphone will call the game back when it has captured at least that much audio data.
    activeMicrophone.BufferDuration = TimeSpan.FromMilliseconds(100);

    // Subscribe to the event that's raised when the capture buffer is filled.
    activeMicrophone.BufferReady += BufferReady;

    // We will put the mic samples in this buffer.  We only want to allocate it once.
    micSamples = new byte[activeMicrophone.GetSampleSizeInBytes(activeMicrophone.BufferDuration)];
    ```

3. The `Buffer Ready` handler can then process the prepared microphone data for storage, transmission or playback, getting the sample data from the Microphone and storing it in the prepared buffer array (micSamples).

    ```csharp
    /// <summary>
    /// This is called each time a microphone buffer has been filled.
    /// </summary>
    void BufferReady(object sender, EventArgs e)
    {
        try
        {
            // Copy the captured audio data into the pre-allocated array.
            int sampleDataSize = activeMicrophone.GetData(micSamples);
        }
        catch (NoMicrophoneConnectedException)
        {
            // Microphone was disconnected - let the user know.
        }
    }
    ```

4. Begin recording audio, calling method [Microphone.Start](xref:Microsoft.Xna.Framework.Audio.Microphone).

    ```csharp
    private void StartRecording()
    {
        // Can't start a microphone that doesn't exist.
        if (activeMicrophone == null) { return; }

        try
        {
            activeMicrophone.Start();
        }
        catch (NoMicrophoneConnectedException)
        {
            // Microphone was disconnected - let the user know.
        }
    }
    ```

5. While the microphone is recording, [Microphone.GetData (Byte\[\], Int32, Int32)](xref:Microsoft.Xna.Framework.Audio.Microphone) is called in the aforementioned `BufferReady` handler as the audio data becomes available.

    ```csharp
    int sampleDataSize = activeMicrophone.GetData(micSamples);
    ```

6. When you are finished recording, set the microphone state to stopped by calling method [Stop](xref:Microsoft.Xna.Framework.Audio.Microphone).

    ```csharp
    private void StopRecording()
    {
        // Can't stop a microphone that doesn't exist.
        if (activeMicrophone == null) { return; }

        try
        {
            // Stop the microphone
            activeMicrophone.Stop();
        }
        catch (NoMicrophoneConnectedException)
        {
            UpdateMicrophoneStatus();
        }
    }
    ```

## Part 2, Full example of outputting the microphone to audio

As promised, the following is a fully working sample that:

* Starts recording and outputting audio from the active microphone when "space is pressed".
* Stops recording when "P" is pressed.
* Adds an "Extension method" to determine if a Microphone is connected.
* A "PickFirstConnectedMicrophone" method to choose a specific microphone.  Can be modified so you can switch mic's
* An "InitializeMicrophone" method to initialize and setup the microphone, updated to work in "Update" so it can continually monitor the Mic connection.

Enjoy, all code commented where applicable for reference.

[!code-csharp[](files/microphonesample.cs)]

## See Also

* [Working with Microphones](HowTo_Microphone.md)

  Provides basic information about microphone usage in games.

* [HowTo_StreamDataFromWav](HowTo_StreamDataFromWav.md)

  a sample demonstrating the uses of the [DynamicSoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.DynamicSoundEffectInstance)

## Reference

* [Microphone](xref:Microsoft.Xna.Framework.Audio.Microphone)

  Provides properties, methods, and fields and events for capturing audio data with microphones.

* [DynamicSoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.DynamicSoundEffectInstance)

  Provides properties, methods, and events for play back of the audio buffer.
