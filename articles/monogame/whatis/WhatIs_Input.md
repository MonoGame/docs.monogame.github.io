---
title: Overview of User Input and Input Devices
description: Input is a general term referring to the process of receiving actions from the user.
---

# Overview of User Input and Input Devices

Input is a general term referring to the process of receiving actions from the user. In MonoGame, the [Microsoft.Xna.Framework.Input](xref:Microsoft.Xna.Framework.Input) namespace provides support for most input devices.

Methods related to input devices unavailable on the platform your game is running on are always available to your code. For example, you can access all [GamePad](xref:Microsoft.Xna.Framework.Input.GamePad) methods on Mobile, but they will not return valid information (unless a GamePad is connected). Although using these methods will not cause exceptions or build errors in your code, they will silently fail when run.

Physical keyboards may or may not be present on mobile devices; you should not rely on the presence of a physical keyboard. For text input, you should use a software input panel (SIP), which will work on all devices, including those with physical keyboards.

If the mobile device does have access to a physical keyboard, the same methods used for keyboards on desktop can be used, given a few caveats.

For multi-touch devices, you can use the raw touch data provided by the [TouchPanel](xref:Microsoft.Xna.Framework.Input.Touch.TouchPanel) class, but you can also use MonoGame's support for predefined gestures if your input fits one of the supported gesture types. For information about working with raw multi-touch input, see [Working with Touch Input](../howto/input/HowTo_UseMultiTouchInput.md). For information about gesture support, see [Detecting Gestures on a Multi-touch Screen](../howto/input/HowTo_Detect_Gestures.md).

The microphone on mobile can be used to capture audio that can be used in your game. For more information, see [Recording Sounds with Microphones](../howto/audio/HowTo_Microphone.md).

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
