---
title: Advanced 2D Shaders in MonoGame
description: A tutorial series on creating advanced 2D visual effects with custom shaders in MonoGame.
---

This tutorial series picks up where the "Building 2D Games" tutorial left off, diving deep into the world of custom shaders. The goal is to develop tooling for MonoGame to facilitate shader development, write some custom effects for _DungeonSlime_, and build intuition for how to approach shader programming.

## What We Will Build

Throughout this series, we will take our _Dungeon Slime_ game and add a whole new layer of visual polish. By the end, you will have implemented a variety of advanced shader effects, including:

- A scene transition effect,
- A color swap effect,
- A 3d effect using `SpriteBatch`,
- A lighting system using techniques from deferred rendering,
- A shadow system using stencil buffers.

![_Dungeon Slime_ will look like this at the end of this series](./videos/final.mp4)

## Who This Is For

This documentation is for intermediate MonoGame users. It assumes that you have a solid understanding of C# and have _already completed the entire "Building 2D Games" tutorial series._

> [!IMPORTANT]
> The concepts and code from the original tutorial, especially the "Shaders" chapter, are a mandatory prerequisite. We will be building directly on top of the final project from that series.

## Table of Contents

This documentation is organized to be read sequentially, as each chapter builds upon the techniques and code from the previous one.

| Chapter                                                              | Summary                                                                                                                                  | Source Files                                                                                                                                       |
| -------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------- |
| [Chapter 01: Introduction](01_introduction/index.md)                 | An overview of the advanced shader techniques covered in this tutorial series.                                                           | [Final Chapter from Building-2D-Games Series](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/learn-monogame-2d/src/24-Shaders/) |
| [Chapter 02: Hot Reload](02_hot_reload/index.md) | Set up a workflow with a "hot-reload" system to recompile and view shader changes in real-time without restarting the game.              | [02-Hot-Reload](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/2dShaders/src/02-Hot-Reload-System/)                   |
| [Chapter 03: The Material Class](03_the_material_class/index.md)     | Create a `Material` class to manage shader parameters and handle the complexities of the shader compiler and our hot-reload system.      | [03-Material-Class](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/2dShaders/src/03-The-Material-Class)                         |
| [Chapter 04: Debug UI](04_debug_ui/index.md)                         | Integrate ImGui.NET to build a real-time debug UI, allowing for runtime manipulation of shader parameters.                               | [04-Debug-UI](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/2dShaders/src/04-Debug-UI)                                         |
| [Chapter 05: Transition Effect](05_transition_effect/index.md)       | Create a flexible, texture-driven screen wipe for smooth scene transitions.                                                              | [05-Transition-Effect](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/2dShaders/src/05-Transition-Effect)                       |
| [Chapter 06: Color Swap Effect](06_color_swap_effect/index.md)       | Implement a powerful color-swapping system using a 1D texture as a Look-Up Table (LUT), allowing for dynamic color palette changes.      | [06-Color-Swap-Effect](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/2dShaders/src/06-Color-Swap-Effect)                       |
| [Chapter 07: Sprite Vertex Effect](07_sprite_vertex_effect/index.md) | Dive into the vertex shader to manipulate sprite geometry, break out of the 2D plane, and give the game a dynamic 3D perspective.        | [07-Sprite-Vertex-Effect](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/2dShaders/src/07-Sprite-Vertex-Effect)                 |
| [Chapter 08: Light Effect](08_light_effect/index.md)                 | Build a 2D dynamic lighting system from scratch using a deferred rendering approach, complete with color, light, and normal map buffers. | [08-Light-Effect](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/2dShaders/src/08-Light-Effect)                                 |
| [Chapter 09: Shadows Effect](09_shadows_effect/index.md)             | Add dynamic 2D shadows to the lighting system by using a vertex shader and the stencil buffer for efficient light masking.               | [09-Shadows-Effect](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/2dShaders/src/09-Shadows-Effect/)                            |
| [Chapter 10: Conclusion & Next Steps](10_next_steps/index.md)        | Review the techniques learned throughout the series and preview further graphics programming topics to continue your journey.            |                                                                                                                                                    |

## Get Started

- [Chapter 01: Introduction](01_introduction/index.md)