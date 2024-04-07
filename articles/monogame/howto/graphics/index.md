---
title: How to
description: A series of articles on how to do certain tasks in MonoGame!
---

# "How To" Articles for MonoGame

These articles provide a details on how to use the Graphics API in MonoGame.

## In This Section

This section demonstrates several graphical concepts divided into three categories:

* [2D Sprite Rendering](#2d-sprite-rendering)
* [Cameras](#cameras)
* [3D Rendering](#3d-rendering)
* [3D Collisions](#3d-collisions)

### 2D Sprite Rendering

This section walks through several core concepts related to sprite rendering, including drawing text to the screen.

[How To Animate A Sprite](HowTo_Animate_Sprite.md)

Demonstrates how to animate a sprite from a texture using a custom class.

[How To Draw A Sprite](HowTo_Draw_A_Sprite.md)

Demonstrates how to draw a sprite by using the SpriteBatch class.

[How To Draw A Sprite Background](HowTo_Draw_Sprite_Background.md)

Demonstrates how to draw a foreground and background sprite using the SpriteBatch class, where only part of the foreground sprite masks the background.

[How To Draw A Sprite Over A Model](HowTo_Draw_Sprite_Over_Model.md)

Demonstrates how to draw a sprite so that it obscures a model. In this example, we are drawing an animated sprite representing an explosion over the current screen position of a 3D model.

[How To Draw Text](HowTo_Draw_Text.md)

Demonstrates how to import a SpriteFont into a project and to draw text using the DrawString method.

[How To Make A Scrolling Background](HowTo_Make_Scrolling_Background.md)

Demonstrates how to draw a scrolling background sprite using the SpriteBatch class.

[How To Rotate A Sprite](HowTo_Rotate_Sprite.md)

Demonstrates how to rotate a sprite around its center.

[How To Rotate A Sprite Group](HowTo_Rotate_Sprite_Group.md)

Demonstrates how to rotate a group of sprites around a single point.

[How To Scale A Sprite](HowTo_Scale_Sprite.md)

Demonstrates how to scale a sprite using a uniform scale.

[How To Scale A Sprite using A Matrix](HowTo_Scale_Sprites_Matrix.md)

Demonstrates how to scale sprites using a matrix that is created based on the viewport width.

[How To Tile Sprites](HowTo_Tile_Sprites.md)

Demonstrates how to draw a sprite repeatedly in the x and y directions in one Draw call.

[How To Tint A Sprite](HowTo_Tint_Sprite.md)

Demonstrates how to tint a sprite using a Color value.

### Cameras

This section walks through several core concepts related to camera operations with MonoGame, including working with multiple screens/viewports.

[How to position the Camera](HowTo_FitCameraToScene.md)

Demonstrates how to position the camera so that all objects in a scene are within the view frustum while maintaining the camera's original orientation.

[How to create a Full-Screen Game](HowTo_FullScreen.md)

Demonstrates how to start a game in full-screen mode.

[How to restrict Aspect Ratio on a Graphics Device](HowTo_AspectRatio.md)

Demonstrates how to create a custom GraphicsDeviceManager that only selects graphics devices with widescreen aspect ratios in full-screen mode.

[How to create a Render Target](HowTo_Create_a_RenderTarget.md)

Demonstrates how to create a render target using the RenderTarget2D class.

[How to display Multiple Screens with Viewports](HowTo_UseViewportForSplitscreenGaming.md)

Demonstrates how to use viewports to display different scenes simultaneously using two cameras.

[Rotating and Moving the Camera](HowTo_RotateMoveCamera.md)

Demonstrates how to rotate and move a camera in a 3D environment. You can rotate the camera about its y-axis, and move it forward and backward. You control the camera's position and orientation by using the directional keys on your keyboard or by using the D-pad of your gamepad.

[How to move the Camera on a Curve](HowTo_ScriptedCamera.md)

Demonstrates how to use the Curve and CurveKey classes to move a camera along the shape of a curve.

### 3D Rendering

This section walks through several core concepts related 3D rendering and math practices with MonoGame.

[How to transform a Point](HowTo_TransformPoint.md)

This example demonstrates how to use the Vector3 and Matrix classes to transform a point. A matrix transform can include scaling, rotating, and translating information.

[How to create a Basic Effect](HowTo_Create_a_BasicEffect.md)

Demonstrates how to create and initialize an instance of the BasicEffect class and use it to draw simple geometry.

[Using a Basic Effect with Texturing](HowTo_Draw_Textured_Quad.md)

Demonstrates how to create and draw a simple quad—two triangles that form a rectangle or square—using **DrawUserIndexedPrimitives**.

[How to render a Model using a Basic Effect](HowTo_RenderModel.md)

Demonstrates how to load and render a model using the MonoGame Content Pipeline. It is assumed that an existing Windows game project is loaded in MonoGame.

[How to enable Anti-aliasing](HowTo_Enable_Anti_Aliasing.md)

Demonstrates how to enable anti-aliasing for your game.

[How to create a State Object](HowTo_Create_a_StateObject.md)

Demonstrates how to create a state object using any of the state object classes: BlendState, DepthStencilState, RasterizerState, or SamplerState.

[How to create a Custom Vertex Declaration](HowTo_UseACustomVertex.md)

Demonstrates how to create a custom vertex declaration and use it to render a 3D object.

[How to Dynamically Update Vertex Data](HowTo_DynamicallyUpdateVertices.md)

Geometry in a 3D game is defined by vertex data. Sometimes, a game needs to modify vertex data or even generate new vertex data dynamically (at run time). Here are some solutions for dynamically updating vertex data.

[Drawing 3D Primitives using Lists or Strips](HowTo_Draw_3D_Primitives.md)

Demonstrates how to draw 3D primitives using lines and triangles arranged as strips or lists.

### 3D Collisions

[Bounding Volumes and Collisions](../HowTo_CollisionDetectionOverview.md)

Collision detection determines whether objects in a game world overlap each other.

[Selecting an Object with a Mouse](HowTo_Select_and_Object_with_a_Mouse.md)

Demonstrates how to check whether the mouse is positioned over a 3D object by creating a ray starting at the camera's near clipping plane and ending at its far clipping plane.

[Testing for Collisions](HowTo_Test_for_Collisions.md)

Demonstrates how to use the BoundingSphere class to check whether two models are colliding.

---

© 2023 The MonoGame Foundation.
