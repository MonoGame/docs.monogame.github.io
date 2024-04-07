---
title: What is the Content Pipeline Architecture?
description: The Content Pipeline is designed to be extensible, so that it can easily support new input file formats and new types of conversion.
---

# What is the Content Pipeline Architecture?

The MonoGame Content Pipeline is a set of processes applied when a game that includes art assets is built. The process starts with an art asset in its original form as a file, and continues to its transformation as data that can be retrieved and used within a MonoGame game through the MonoGame Framework Class Library.

The Content Pipeline is designed to be extensible, so that it can easily support new input file formats and new types of conversion.

Most MonoGame developers can ignore the inner workings of the Content Pipeline. The most commonly used types of game assets and formats are inherently supported by MonoGame. However, if you are a game developer who needs to support a new file format or game-engine capability, it is useful to understand the stages of the Content Pipeline that transform an asset from a digital-content creation (DCC) output file to part of the game binary.

## Content Pipeline Components

A game asset is made available to an MonoGame game after it is added to the Content project. Once the asset is part of the game solution, it is included in the Content Pipeline.

Processes fall into two types depending on when they execute: design time components and runtime components.

### Design-Time Components

The design-time components of the MonoGame Content Pipeline that process your game assets execute when you build your MonoGame game as an executable file. These processes perform the initial transformation of an asset from its digital content creation (DCC) format to a managed code object that your game can use upon execution.

Design-time components use the [Content Pipeline Class Library](CP_Class_Library.md), which can be used and extended to create custom Content Pipeline design-time components.

#### Content Importer

A _Content Importer_ converts game assets from a particular DCC file format into objects in the MonoGame Content Document Object Model (DOM) that standard Content Processors can consume, or into some other custom form that a particular custom Content Processor can consume.

An Content Importer typically converts content into managed objects based on the Content DOM, which includes strong typing for such assets as meshes, vertices, and materials. A custom Content Importer, however, may produce custom objects for a particular custom Content Processor to consume.

#### Content Processor

A _Content Processor_ takes one specific type of an imported game asset and compiles it into a managed code object that can be loaded and used by MonoGame games.

Each Content Processor acts upon a specific object type. For example, the [Effect Processor](CP_StdImpsProcs.md#standard-content-processors) accepts only [EffectContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.EffectContent) objects, representing a DirectX Effect asset.

When you include a game asset file in your MonoGame .mgcb file, its **dialog properties** page specifies the appropriate Content Importer and Content Processor. Thereafter, when you build your game (by pressing F5), the assigned Content Importer and Content Processor for each asset is invoked automatically. The asset is built into your game in a form that can be loaded at run time by your game.

The managed code objects created by the Content Processor are serialized into a compact binary format (also referred to as an intermediate format) file with an .XNB extension by the Content Pipeline Content Compiler. This .XNB file is used by the runtime components of the Content Pipeline that assist your game in retrieving the transformed game assets.

The format of data in the .XNB file is tightly coupled to the MonoGame Framework. It is not designed for use by other runtime libraries.

### Runtime Components

Runtime components of the Content Pipeline support loading and using the transformed game asset by your MonoGame game. These components use the [MonoGame library](../WhatIs_MonoGame_Class_Library.md), which can be extended to create custom components.

Content Loader

When the game needs the game asset's managed code object, it must call the [ContentManager.Load](xref:Microsoft.Xna.Framework.Content.ContentManager) method to invoke the Content Loader, specifying the object type it expects. The Content Loader then locates and loads the asset from the compact binary format (.XNB) file into the memory space of the game where it can be used.

## See Also

- [What Is Content?](CP_Overview.md)
- [Adding New Content Types](CP_Content_Advanced.md)
- [Loading Additional Content Types](CP_Customizing.md)

---

© 2012 Microsoft Corporation. All rights reserved.

© 2023 The MonoGame Foundation.
