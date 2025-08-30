---
title: "Chapter 01: Getting Started"
description: "Prepare your project and get ready"
---

Welcome to the advanced 2D shaders tutorial! Before we dive into writing complex effects, this first chapter will make sure you're ready to go. We will get the starting project set up and take a quick look at the technical foundation of how shaders work with MonoGame's `SpriteBatch`.

## The Starting Code

This tutorial series builds directly on top of the final code from the "Building 2D Games" tutorial. It is essential that you start with this project.

> [!note]
> You can get the complete starting source code for this tutorial here: 
> [The final chapter's source code](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/learn-monogame-2d/src/24-Shaders/)

Once you have the code downloaded, open it in your IDE and run the `DungeonSlime` project to make sure everything is working correctly. You should see the title screen from the previous tutorial.

## Project Structure

The solution is organized into two main projects:

- **`DungeonSlime`**: The main game project. This contains our game logic and game-specific content.
- **`MonoGameLibrary`**: Our reusable class library. We will be adding new, generic helper classes here that could be used in any of your future MonoGame projects.

Most of our shader files (`.fx`) will be created in the `Content/effects` folder within the `DungeonSlime` project to start, and later within the `MonoGameLibrary` for shared effects.

## What's Next

Now that our project is set up, we can get to work. The focus for the first several chapters will be to create a workflow for developing shaders in MonoGame. Once we have a hot-reload system, a class to manage the effects, and a debug UI ready, we will carry on and build up 5 effects. The effects will range from simple pixel shaders and vertex shaders up to rendering techniques. As we develop these shaders together, we will build an intuition for how to tackle shader development. 

Continue to the next chapter, [Chapter 02: Hot Reload](../02_hot_reload/index.md)


