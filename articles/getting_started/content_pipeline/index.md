---
title: Adding Content
description: Learn how to add content to your game with the MonoGame Content Pipeline and the Content Builder.
---

A big part of your game is your content. This includes standard files such as textures, sound effects, music, videos and custom effects, as well as custom content like levels and bespoke configuration.

MonoGame has its own content pipeline that transforms your source assets into bundles that are optimized for each platform MonoGame supports. Games load this built content at runtime, which keeps load times short and memory use low on devices with tight resource limits.

> [!NOTE]
> See [Why use the Content Pipeline](why_content_pipeline.md) for the reasons behind this, and what the pipeline does for each type of asset.

## Content Builder

From MonoGame 3.8.6, the Content Builder is the recommended way to build content. It is a console project in your solution that reads your source assets, applies rules that you write in C#, and produces the built content for your game. There are no tools to install and no content file to edit, and the same builder can run on your machine or in an automated workflow.

Just code, no GUI, and freedom of control. It also supports automation out of the box too.

> [!NOTE]
> Being a console project means you can build your content separately and you are not chained down to a specific setup, which caused so many issues with the legacy MGCB editor.

The Content Builder course has four parts:

- [Part 1: Getting Started](content_builder/index.md) explains what the Content Builder is, how to create a project from the StartKit templates, and load your first asset.
- [Part 2: Setting up the Content Builder](content_builder/setup.md) explains how the builder project works, how to write the rules that include, exclude and process your assets, and how to add each type of asset, including fonts, effects and XML.
- [Part 3: Advanced Scenarios and Automation](content_builder/advanced.md) covers building different types of content for each platform, custom importers and processors, and building your content using automation with GitHub Actions.
- [Part 4: Migrating from MGCB](content_builder/migrating_from_mgcb.md) explains how to move an existing project from the legacy MGCB Editor and `.mgcb` files to the Content Builder.

## Legacy MGCB

Projects created before MonoGame 3.8.6 built their content with the MGCB Editor and `.mgcb` files. These pages remain available for those projects:

- [Using MGCB Editor](using_mgcb_editor.md)
- [TrueType Fonts](adding_ttf_fonts.md)
- [Custom Effects](custom_effects.md)
- [Localization](localization.md)
