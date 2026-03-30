---
title: Advanced 2D Shaders in MonoGame
description: A tutorial series on creating advanced 2D visual effects with custom shaders in MonoGame.
---

This tutorial series picks up where the [Building 2D Games](https://docs.monogame.net/articles/tutorials/building_2d_games/24_shaders/index.html) tutorial left off, diving deep into the world of custom shaders. The goal is to develop tooling for MonoGame to facilitate shader development, write some custom effects for _DungeonSlime_, and build intuition for how to approach shader programming.

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
> The concepts and code from the original tutorial, especially the [Shaders](https://docs.monogame.net/articles/tutorials/building_2d_games/24_shaders/index.html) chapter, are a mandatory prerequisite. We will be building directly on top of the final project from that series.

## Table of Contents

This documentation is organized to be read sequentially, as each chapter builds upon the techniques and code from the previous one.

| Chapter                                                              | Summary                                                                                                                                  | Source Files                                                                                                                                       |
| -------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------- |
| [Chapter 01: Introduction](01_introduction/index.md)                 | An overview of the advanced shader techniques covered in this tutorial series.                                                           | [Final Chapter from Building-2D-Games Series](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.5/Tutorials/learn-monogame-2d/src/27-Conclusion/) |
| [Chapter 02: Hot Reload](02_hot_reload/index.md) | Set up a workflow with a "hot-reload" system to recompile and view shader changes in real-time without restarting the game.              | [02-Hot-Reload](https://github.com/MonoGame/MonoGame.Samples/blob/3.8.5/Tutorials/2dShaders/src/02-Hot-Reload-System/)                   |
| [Chapter 03: The Material Class](03_the_material_class/index.md)     | Create a `Material` class to manage shader parameters and handle the complexities of the shader compiler and our hot-reload system.      | [03-Material-Class](https://github.com/MonoGame/MonoGame.Samples/blob/3.8.5/Tutorials/2dShaders/src/03-The-Material-Class/)                         |
| [Chapter 04: Debug UI](04_debug_ui/index.md)                         | Integrate ImGui.NET to build a real-time debug UI, allowing for runtime manipulation of shader parameters.                               | [04-Debug-UI](https://github.com/MonoGame/MonoGame.Samples/blob/3.8.5/Tutorials/2dShaders/src/04-Debug-UI/)                                         |
| [Chapter 05: Transition Effect](05_transition_effect/index.md)       | Create a flexible, texture-driven screen wipe for smooth scene transitions.                                                              | [05-Transition-Effect](https://github.com/MonoGame/MonoGame.Samples/blob/3.8.5/Tutorials/2dShaders/src/05-Transition-Effect/)                       |
| [Chapter 06: Color Swap Effect](06_color_swap_effect/index.md)       | Implement a powerful color-swapping system using a 1D texture as a Look-Up Table (LUT), allowing for dynamic color palette changes.      | [06-Color-Swap-Effect](https://github.com/MonoGame/MonoGame.Samples/blob/3.8.5/Tutorials/2dShaders/src/06-Color-Swap-Effect/)                       |
| [Chapter 07: Sprite Vertex Effect](07_sprite_vertex_effect/index.md) | Dive into the vertex shader to manipulate sprite geometry, break out of the 2D plane, and give the game a dynamic 3D perspective.        | [07-Sprite-Vertex-Effect](https://github.com/MonoGame/MonoGame.Samples/blob/3.8.5/Tutorials/2dShaders/src/07-Sprite-Vertex-Effect/)                 |
| [Chapter 08: Light Effect](08_light_effect/index.md)                 | Build a 2D dynamic lighting system from scratch using a deferred rendering approach, complete with color, light, and normal map buffers. | [08-Light-Effect](https://github.com/MonoGame/MonoGame.Samples/blob/3.8.5/Tutorials/2dShaders/src/08-Light-Effect/)                                 |
| [Chapter 09: Shadows Effect](09_shadows_effect/index.md)             | Add dynamic 2D shadows to the lighting system by using a vertex shader and the stencil buffer for efficient light masking.               | [09-Shadows-Effect](https://github.com/MonoGame/MonoGame.Samples/blob/3.8.5/Tutorials/2dShaders/src/09-Shadows-Effect/)                            |
| [Chapter 10: Conclusion & Next Steps](10_next_steps/index.md)        | Review the techniques learned throughout the series and preview further graphics programming topics to continue your journey.            |                                                                                                                                                    |

## Conventions Used in This Documentation

The following conventions are used in this documentation.

### Italics

*Italics* are used for emphasis and technical terms.

### Highlights

`Highlights` are used for paths such as file paths, including filenames, extensions and other critical information in the application of steps in a tutorial.  These are similar to Inline code blocks as they stand out more in Markdown and require emphasis.

### Inline Code Blocks

`Inline code` blocks are used for methods, functions, and variable names when they are discussed within the paragraphs of text.  For example, when referring to a method such as `Foo` in a sentence, it will appear with this formatting.

### Code Blocks

```cs
// Example Code Block
public void Foo() { }
```

Code blocks are used to show code examples with syntax highlighting.  These appear as separated blocks rather than inline with text, allowing for multi-line examples and better readability of complete code snippets.

## MonoGame

If you ever have questions about MonoGame or would like to talk with other developers to share ideas or just hang out with us, you can find us in the various MonoGame communities below:

- [Discord](https://discord.gg/monogame)
- [GitHub Discussions Forum](https://github.com/MonoGame/MonoGame/discussions)
- [Community Forums (deprecated)](https://community.monogame.net/)
- [Reddit](https://www.reddit.com/r/monogame/)
- [Facebook](https://www.facebook.com/monogamecommunity)

## Note From Author

> My name is [Chris Hanna](https://github.com/cdhanna) (notably, I am _not_ Christopher Whitley, who wrote the wonderful _Building 2D Games_ series). I have very fond memories of using MonoGame back in 2010 ([when it was called XNA](https://docs.monogame.net/articles/tutorials/building_2d_games/01_what_is_monogame/index.html#a-brief-history)) to put code _I wrote_ (!) onto my humble Xbox 360. My young mind was delighted. I have been a life long lover of game development. Before XNA, I learned to program by using [Dark Basic Pro](https://github.com/Dark-Basic-Software-Limited/Dark-Basic-Pro). Then XNA taught me how to use "real" languages like C#. Learning C# eventually led me to the Unity game engine, which helped me find a job in the gaming industry in 2019 at a game studio called Disruptor Beam (which has become a new company called [Beamable](https://beamable.com/)). I owe a lot to tools like MonoGame, and the communities that build and maintain them.
>
> I drifted away from MonoGame around 2018, finding myself using tools like Unity instead. Then, I found my way back to MonoGame after the [Unity Runtime Fee debacle of 2023](https://www.pocketgamer.biz/the-unity-runtime-fee-farago-the-whole-story-all-in-one-place/). Like many game developers, I wanted to find a game framework that I could trust and rely on. MonoGame was the top of my list. I was even able to [contribute](https://github.com/MonoGame/MonoGame/pull/8456) to MonoGame, by helping upgrade the texture compression tools from legacy XNA-era tools to modern standards.
>
> I had always wanted to learn computer graphics and shaders, but for years, I had never been able to build the fundamental intuition. I remember trying to learn shaders at the same time I was learning C#, back in the 2010-era, and I could not make heads or tails of it all. I tried to learn shaders _again_ a few years later and still struggled to really grasp the foundations of what a shader _is_. To be honest, I owe my current understanding of shaders to tools like [Shader Toy](https://www.shadertoy.com/), and specifically, the youtube channel, [_The Art Of Code_](https://www.youtube.com/@TheArtofCodeIsCool). The way the author, Martijn, explained the basic concepts of shader mathematics was exactly what my brain needed to see. All of his tutorials started with a tiny template shader, and through many (many) small incremental updates, he would always arrive at a shader that produced beautiful imagery on the screen. Procedural generation shaders are one of my favorite things. There is truly something magical about seeing complex visuals produced by the construction of simple arithmetics. My knowledge of shaders ballooned after that.
>
> I hear a lot of people say, "shaders are magic". They _are_ magical, but _we_ are the wizards! The truly magical thing about shaders is that you can wield their power to create wonderful things. I hope that this shader tutorial will help you connect some of the basic intuitions in your brain, and that you come to enjoy shader programming as much as I do.
>
> [Chris Hanna](https://github.com/cdhanna),
>
> [https://brewed.ink](https://brewed.ink)

### Acknowledgements

This documentation would not have been possible without the support and contributions from numerous individuals within the MonoGame community.

I would like to acknowledge that this tutorial series would not be possible without the help and guidance of many people. The entire MonoGame foundation deserves endless credit for keeping MonoGame active and relevant, and _specifically_, [Simon Jackson](https://github.com/SimonDarksideJ) needs immense gratitude for editing and reviewing this tutorial. I also think that [Christopher Whitley](https://github.com/AristurtleDev) merits on-going applause for creating the original tutorial content. Lastly, MonoGame exists because there is a strong community of developers who love using MonoGame.

## Get Started

- [Chapter 01: Introduction](01_introduction/index.md)
