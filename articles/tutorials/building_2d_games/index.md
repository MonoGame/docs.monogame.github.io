---
title: Building 2D Games with MonoGame
description: A beginner tutorial for building 2D games with MonoGame.
---

This tutorial covers game development concepts using the MonoGame framework as our tool.  Throughout each chapter, we will explore game development with MonoGame, introducing new concepts to build upon previous ones as we go. we will create a Snake game, which will serve as the vehicle to apply the concepts learned throughout the tutorial.  The goal of this tutorial is to give you a solid foundation in 2D game development with MonoGame and provide you with reusable modules that you can jump start future projects.

## Who This Is For

This documentation is meant to be an introduction to game development and MonoGame. Readers should have a foundational understanding of C# and be comfortable with concepts such as classes and objects.

> [!NOTE]
> If you are just getting started with C# for the first time, I would recommend following the official [Learn C#](https://dotnet.microsoft.com/en-us/learn/csharp) tutorials provided by Microsoft. These are free tutorials that teach you programming concepts as well as the C# languages. Once you feel you have a good foundation with that, come back and continue here.

## How This Documentation Is Organized

This documentation will introduce game development concepts using the MonoGame framework while walking the reader through the development of a Snake clone. The documentation is organized such that each chapter should be read sequentially, with each introducing new concepts and building off of the previous chapters.

> [!CAUTION]
> This is currently a work in progress and is not finished.

| Chapter                                                                      | Summary                                                                                                                                                                                           | Source Files |
|------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------|
| [01: What Is MonoGame](01_what_is_monogame/index.md)                         | Learn about the history of MonoGame and explore the features it provides developers when creating games.                                                                                          |              |
| [02: Getting Started](02_getting_started/index.md)                           | Setup your development environment for dotnet development and MonoGame using Visual Studio Code as your IDE.                                                                                      |              |
| [03: The Game1 File](03_the_game1_file/index.md)                             | Explore the contents of the Game1 file generated when creating a new MonoGame project.                                                                                                            |              |
| [04: Content Pipeline](04_content_pipeline/index.md)                         | Learn the advantages of using the **Content Pipeline** to load assets and go through the processes of loading your first asset                                                                    |              |
| [05: Working with Textures](05_working_with_textures/index.md)               | Learn how to load and render textures using the MonoGame content pipeline and [**SpriteBatch**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch).                                               |              |
| [06: Optimizing Texture Rendering](06_optimizing_texture_rendering/index.md) | Explore optimization techniques when rendering textures using a texture atlas.                                                                                                                    |              |
| [07: The Sprite Class](07_the_sprite_class/index.md)                         | Explore creating a reusable Sprite class to efficiently sprites and their rendering properties, including position, rotation, scale, and more.                                                    |              |
| [08: The AnimatedSprite Class](07_the_sprite_class/index.md)                 | Create an AnimatedSprite class that builds upon our Sprite class to support frame-based animations.                                                                                               |              |
| [09: Handling Input](09_handling_input/index.md)                             | Learn how to handle keyboard, mouse, and gamepad input in MonoGame.                                                                                                                               |              |
| [10: Input Management](10_input_management/index.md)                         | Learn how to create an input management system to handle keyboard, mouse, and gamepad input, including state tracking between frames and creating a reusable framework for handling player input. |              |
| [11: Collision Detection](11_collision_detection/index.md)                   | Learn how to implement collision detection between game objects and handle collision responses like blocking, triggering events, and bouncing.                                                    |              |

In additional to the chapter documentation, supplemental documentation is also provided to give a more in-depth look at different topics with MonoGame. These are provided through the Appendix documentation below:

| Appendix | Summary |
|----------|---------|
|          |         |

## Conventions Used in This Documentation

The following conventions are used in this documentation

### Italics

*Italics* are used for emphasis, technical terms, and paths such as file paths including filenames and extensions.

### Inline Code Blocks

`Inline code` blocks are used for methods, functions, and variable names when they are discussed with the body a text paragraph.

### Code Blocks

```cs
// Example Code Block
public void Foo() { }
```

Code blocks are used to show code examples with syntax highlighting

## MonoGame

If you ever have questions about MonoGame or would like to talk with other developers to share ideas or just hang out with us, you can find us in the various MonoGame communities below

* [Discord](https://discord.gg/monogame)
* [GitHub Discussions Forum](https://github.com/MonoGame/MonoGame/discussions)
* [Community Forums (deprecated)](https://community.monogame.net/)
* [Reddit](https://www.reddit.com/r/monogame/)
* [Facebook](https://www.facebook.com/monogamecommunity)

## Note From Author

> I have been using MonoGame for the past several years (since 2017). It was a time in my game development journey where I was looking for something that I had more control over. I didn't want to spend the time to write a full game engine, but I also wanted to have more control than what the current engines at the time (i.e. Unity) offered. At that time, there was a vast amount of resources available for getting started, but none of them felt like they fit a good beginner series. Even now, the resources available still seem this way. They either require the reader to have a great understanding of game development and programming, or they assume the reader has none and instead focuses on teaching programming more than teaching MonoGame. Even still, some relied too heavily on third party libraries, others were simply very bare bones asking the reader to just copy and paste code without explaining the *what* of it all.
> 
> Since then, I have written various small self contained tutorials on different topics for MonoGame to try and give back to the community for those getting started. I also participate regularly in the community discussion channels, answering questions and offering technical advice, so I'm very familiar with the topics and pain points that users get hung up on when first starting out.
> 
> With this documentation, I hope to take the lessons I've learned and provide a tutorial series that I wish was available when I first started.. To present using MonoGame in a straight forward way, introducing concepts and building off of them as we go along in a way that makes sense and is easy to follow.
>
> \- Christopher Whitley (Aristurtle)

## Acknowledgements

> [!NOTE]
> Acknowledgments will be added at a later time to recognize everyone that assisted with editing and reviewing this documentation.