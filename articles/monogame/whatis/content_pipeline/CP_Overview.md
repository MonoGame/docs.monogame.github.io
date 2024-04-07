---
title: What Is Content?
description: Content is all the parts of your game that are not executing managed code. It includes all art assets, such as textures, sprites, meshes, effects, and fonts; and includes sound assets, such as music or brief sound effects. It also can include data assets, such as tables of levels or character attributes.
---

# What Is Content?

The MonoGame Content Pipeline is a set of processes applied to a game's art and data assets when the game is built. The process starts with an art asset in its original form as a file, and it continues to its transformation as data that can be retrieved and used within a MonoGame game through the MonoGame Framework Class Library.

Content is all the parts of your game that are not executing managed code. It includes all art assets, such as textures, sprites, meshes, effects, and fonts; and includes sound assets, such as music or brief sound effects. It also can include data assets, such as tables of levels or character attributes.

Most content will be created using a digital content creation (DCC) tool, such as a paint program or a 3D model editor. The content your game uses is stored in a variety of file formats. Most will be standard file formats, such as JPEG for textures or Autodesk FBX format for 3D models. However, they might also be in a custom format or house custom data in XML format.

## Benefits of the Content Pipeline

The chief reason MonoGame uses a Content Pipeline is to help your game run fast. Without the content pipeline, your game would have to be built with its art assets in their original file format. When the game needs to load its art to draw it on the screen, it would have to determine its format and convert the data into a form it can use more directly. This would have to be performed at run time, for each asset, making the player wait to have fun.

The Content Pipeline remedies this by shifting this time-consuming work to when the game is built. At build time, each asset is imported from its original file format and processed into a managed code object. Those objects are then serialized to a file that is included in the game’s executable.

At run time, the game can then read the serialized data from the file directly into a managed code object for immediate use.

This architecture also provides several other benefits.

- Game artists can use the DCC tools of their choice.
- Game developers can use the files game artists produce directly in their MonoGame projects.
- If all game assets are in file formats supported by the [Standard Importers and Processors](CP_StdImpsProcs.md) provided by MonoGame, the game developer never needs to be concerned with the specifics of that file format, nor possess a detailed knowledge of how the Content Pipeline works.
- If required, the Content Pipeline can easily be customized or extended to import a new file format or to produce custom output.

## Using the Content Pipeline

For MonoGame games that use the most common types of art assets and formats, a detailed knowledge of how the Content Pipeline works is unnecessary. MonoGame supplies standard importers and processors for many popular DCC file formats. The only procedure necessary is to add these assets to the game's content project. For a list of formats MonoGame inherently supports, see [Standard Content Importers and Content Processors](CP_StdImpsProcs.md).

To use a DirectX Effect (.fx) file in your game, for example, just [add](../../howto/Content_Pipeline/HowTo_GameContent_Add.md) the file to the MonoGame game content project (.mgcb). The MonoGame Pipeline tool recognizes the .fx file type as one it supports, and it assigns the correct components to it that are used to process it when the game is built. The game then can access the effect through the standard [ContentManager.Load](xref:Microsoft.Xna.Framework.Content.ContentManager) method of the MonoGame Framework. The game programmer does not need to be concerned about all the steps taken by the Content Pipeline to ready it for this straightforward use.

There are other circumstances, however, when it helps to understand how the Content Pipeline works.

- A third party may provide custom MonoGame Content Pipeline components that support additional art assets and formats.
- You may need to write your own custom MonoGame Content Pipeline components to support a new type of art asset or format from a DCC.
- You may wish to write your own custom MonoGame Content Pipeline components to derive special-purpose content from another piece of content at the time the game is built.

## Content Pipeline Components

The processes that comprise the MonoGame Content Pipeline fall into two types, depending on when they execute: build-time components and run-time components.

### Build-Time Components

The build-time components of the Content Pipeline that process your art assets execute within Visual Studio when you build your MonoGame game and produce an executable file. These processes perform the initial transformation of an asset from a DCC format to a managed code object that your game can use when it executes.

These build-time components make use of the [Content Pipeline Class Library](CP_Class_Library.md), which can be used and extended to create custom Content Pipeline build-time components.

|Component|Description|
|-|-|
|Importer|An _importer_ converts art assets from a particular DCC file format to objects in the MonoGame [Content Document Object Model](CP_DOM.md) that standard content processors can consume, or to some other custom form that a particular custom processor can consume.|
|Content processor|A _processor_ takes one specific type of imported art asset, such as a set of meshes, and compiles it into a managed code object that can be loaded and used by MonoGame games at runtime.|

When you include an art asset file in your MonoGame solution's content project (.mgcb), its Properties page in the Pipeline tool specifies the appropriate importer and processor. Thereafter, when you build your game (by pressing F5), the assigned importer and processor for each asset is invoked automatically. The asset is built into your game in a form that can be loaded at run time by your game by using [ContentManager.Load](xref:Microsoft.Xna.Framework.Content.ContentManager).

## Run-time Components

The run-time components of the MonoGame Content Pipeline support the loading and use of the transformed art asset by your MonoGame.

These run-time components make use of the [MonoGame Framework Class Library](../WhatIs_MonoGame_Class_Library.md), which can be extended to create custom Content Pipeline run-time components.

## See Also

### Concepts

- [Adding Content to a Game](index.md)  
- [Adding New Content Types](CP_Content_Advanced.md)  

---

© 2012 Microsoft Corporation. All rights reserved.

© 2023 The MonoGame Foundation.
