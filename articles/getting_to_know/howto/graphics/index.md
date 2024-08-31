---
title: How to articles for the Graphics pipeline
description: These articles provide a details on how to use the Graphics API in MonoGame.
requireMSLicense: true
---

## In This Section

This section demonstrates several graphical concepts divided into the following categories:

* [2D Sprite Rendering](#2d-sprite-rendering)
* [Cameras](#cameras)
* [3D Rendering](#3d-rendering)
* [3D Collisions](#3d-collisions)

### 2D Sprite Rendering

This section walks through several core concepts related to sprite rendering, including drawing text to the screen.

- [How To Draw A Sprite](HowTo_Draw_A_Sprite.md)

  Demonstrates how to draw a sprite by using the SpriteBatch class.

- [How To Draw A Sprite Background](HowTo_Draw_Sprite_Background.md)

  Demonstrates how to draw a foreground and background sprite using the SpriteBatch class, where only part of the foreground sprite masks the background.

- [How To Tint A Sprite](HowTo_Tint_Sprite.md)

  Demonstrates how to tint a sprite using a Color value.

- [How To Rotate A Sprite](HowTo_Rotate_Sprite.md)

  Demonstrates how to rotate a sprite around its center.

- [How To Rotate A Sprite Group](HowTo_Rotate_Sprite_Group.md)

  Demonstrates how to rotate a group of sprites around a single point.

- [How To Scale A Sprite](HowTo_Scale_Sprite.md)

  Demonstrates how to scale a sprite using a uniform scale.

- [How To Scale A Sprite using A Matrix](HowTo_Scale_Sprites_Matrix.md)

  Demonstrates how to scale sprites using a matrix that is created based on the viewport width.

- [How To Tile Sprites](HowTo_Tile_Sprites.md)

  Demonstrates how to draw a sprite repeatedly in the x and y directions in one Draw call.

- [How To Draw Text](HowTo_Draw_Text.md)

  Demonstrates how to import a SpriteFont into a project and to draw text using the DrawString method.

- [How To Animate A Sprite](HowTo_Animate_Sprite.md)

  Demonstrates how to animate a sprite from a texture using a custom class.

- [How To Make A Scrolling Background](HowTo_Make_Scrolling_Background.md)

  Demonstrates how to draw a scrolling background sprite using the SpriteBatch class.

> More Coming soon

### Cameras

> Coming soon

- [Rotating and Moving the Camera](HowTo_RotateMoveCamera.md)

  Demonstrates how to rotate and move a camera in a 3D environment. You can rotate the camera about its y-axis, and move it forward and backward. You control the camera's position and orientation by using the directional keys on your keyboard or by using the D-pad of your gamepad.

- [How to position the Camera](HowTo_FitCameraToScene.md)

  Demonstrates how to position the camera so that all objects in a scene are within the view frustum while maintaining the camera's original orientation.

- [How to create a Render Target](HowTo_Create_a_RenderTarget.md)

  Demonstrates how to create a render target using the RenderTarget2D class.

### 3D Rendering

This section walks through several core concepts related 3D rendering and math practices with MonoGame.

- [How to transform a Point](HowTo_TransformPoint.md)

  This example demonstrates how to use the Vector3 and Matrix classes to transform a point. A matrix transform can include scaling, rotating, and translating information.

- [How to create a Basic Effect](HowTo_Create_a_BasicEffect.md)

  Demonstrates how to create and initialize an instance of the BasicEffect class and use it to draw simple geometry.

- [How to create a State Object](HowTo_Create_a_StateObject.md)

  Demonstrates how to create a state object using any of the state object classes: BlendState, DepthStencilState, RasterizerState, or SamplerState.

- [Drawing 3D Primitives using Lists or Strips](HowTo_Draw_3D_Primitives.md)

  Demonstrates how to draw 3D primitives using lines and triangles arranged as strips or lists.

- [How to render a Model using a Basic Effect](HowTo_RenderModel.md)

  Demonstrates how to load and render a model using the MonoGame Content Pipeline. It is assumed that an existing Windows game project is loaded in MonoGame.

- [How to enable Anti-aliasing](HowTo_Enable_Anti_Aliasing.md)

  Demonstrates how to enable anti-aliasing for your game.


### 3D Collisions

> Coming soon
