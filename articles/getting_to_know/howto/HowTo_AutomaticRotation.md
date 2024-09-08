---
title: How to manage automatic rotation and scaling
description: A walkthrough what is involved in figuring out if two objects collide for MonoGame!
requireMSLicense: true
---

## Overview

This topic describes automatic rotation and scaling in the MonoGame Framework. Rotation and scaling are done in hardware at no performance cost to the game.

### Setting the Preferred Device Orientations

If your game supports more than one display orientation, as specified by [SupportedOrientations](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.SupportedOrientations) and described with [DisplayOrientation](xref:Microsoft.Xna.Framework.DisplayOrientation), the MonoGame Framework automatically rotates and scales the game when the `OrientationChanged` event is raised.

### When the device's orientation is changed

The current back buffer resolution is scaled, and can be queried by using [PreferredBackBufferWidth](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.PreferredBackBufferWidth) and [PreferredBackBufferHeight](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.PreferredBackBufferHeight). These values will not be the same as the non-scaled screen resolution, which can be queried by using [DisplayMode](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice.DisplayMode) or [ClientBounds](xref:Microsoft.Xna.Framework.GameWindow.ClientBounds).

## Orientation defaults

If you leave [SupportedOrientations](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.SupportedOrientations) set to **DisplayOrientation.Default**, orientation is automatically determined from your [PreferredBackBufferWidth](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.PreferredBackBufferWidth) and [PreferredBackBufferHeight](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.PreferredBackBufferHeight). If the [PreferredBackBufferWidth](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.PreferredBackBufferWidth) is greater than the [PreferredBackBufferHeight](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.PreferredBackBufferHeight), the game will run in the landscape orientation and automatically switch between **LandscapeLeft** and **LandscapeRight** depending on the position which the user holds the phone. To run a game in the portrait orientation, set the [PreferredBackBufferWidth](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.PreferredBackBufferWidth) to a value smaller than the [PreferredBackBufferHeight](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.PreferredBackBufferHeight).

## See Also

- [XNA Orientation Sample](https://github.com/simondarksidej/XNAGameStudio/wiki/Orientation)

### Concepts

- [What Is a Back Buffer?](WhatIs_BackBuffer.md)

### Reference

- [SupportedOrientations](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.SupportedOrientations)
- [DisplayOrientation](xref:Microsoft.Xna.Framework.DisplayOrientation)
