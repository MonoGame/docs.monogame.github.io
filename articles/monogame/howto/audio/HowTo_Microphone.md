---
title: How to work with Microphones
description: This topic provides basic information about microphone usage in games.
---

# Working with Microphones

This topic provides basic information about microphone usage in games.

## Supported Microphone Devices

The Microphone API is only implemented on OpenAL based platforms at this time. This includes iOS/Android and Desktop projects using DesktopGL.

## Capabilities of the Microphone API

The MonoGame Microphone API has the following functionality:

* Captures the audio stream from a microphone.
* Submits and controls a stream of audio buffers for playback using the [DynamicSoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.DynamicSoundEffectInstance) object.
* Plays back audio.

## Microphone API Process Workflow

The Microphone API behaves like a simple audio recorder with a configurable capture buffer. It has been designed with the following development process workflow:

1. Select the microphone connected to the device.
2. Configure the microphone's capture buffer size.
3. Control the recording using standard transport controls (Start and Stop).
4. Retrieve the captured audio using the [GetData](xref:Microsoft.Xna.Framework.Audio.Microphone) method.

Also, you can use the **BufferReady** event handler of the [Microphone](xref:Microsoft.Xna.Framework.Audio.Microphone) class if you want to be notified when the audio capture buffer is ready to be processed.

## Concepts

[Creating and Playing Sounds](../../whatis/WhatIs_Audio.md)

Provides overviews about audio technology, and presents predefined scenarios to demonstrate how to use audio.

## Reference

[DynamicSoundEffectInstance](xref:Microsoft.Xna.Framework.Audio.DynamicSoundEffectInstance)

Provides properties, methods, and events for play back of the audio buffer.

[Microphone](xref:Microsoft.Xna.Framework.Audio.Microphone)

Provides properties, methods, and fields and events for capturing audio data with microphones.

---

© 2012 Microsoft Corporation. All rights reserved.

© 2023 The MonoGame Foundation.
