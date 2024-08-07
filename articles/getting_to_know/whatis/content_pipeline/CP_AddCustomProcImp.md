---
title: What is  a Custom Importer?
description: MonoGame provides standard importers and processors for a number of common file formats used to store such basic game assets as models, materials effects, sprites, textures, and so on. For a list of file formats supported by these standard importers and processors.
requireMSLicense: true
---

MonoGame provides standard importers and processors for a number of common file formats used to store such basic game assets as models, materials effects, sprites, textures, and so on. For a list of file formats supported by these standard importers and processors, see [Standard Content Importers and Content Processors](CP_StdImpsProcs.md).

## To add a custom importer or processor to a game project

This procedure assumes you have copied the new importer or processor to a local folder in the game project.

1. Open the MonoGame Pipeline Tool.
2. Load the `.mgcb` file associated with your game.
3. Click on the `Content` node and in the `Properties panel` click on `References`.
4. Click the `Add` Button.
5. Navigate to the directory containing the assembly with the custom importer or processor, and then add it.
6. Save.

> [!IMPORTANT]
> For MonoGame `3.8.2` and below, make sure to keep the project at `.NET 6` or below.  The MGCB tool for 3.8.2 CANNOT understand or read .NET 8 libraries as it is compiled with .NET 6.  The Game project can use a higher version, but library projects must stay at `.NET 6` else they cannot be read.

The new importer or processor now appears as a choice in dialog properties for importing or processing a newly added game asset.

## See Also

- [Adding New Content Types](CP_Content_Advanced.md)  
- [Standard Content Importers and Content Processors](CP_StdImpsProcs.md)  
