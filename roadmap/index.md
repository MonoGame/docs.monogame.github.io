---
title: MonoGame Roadmap
description: The forecasted roadmap for MonoGame development/improvement, including lists of bounties in development.
_disableToc: true
_disableBreadcrumb: true
---

## MonoGame official roadmap

![Roadmap](./images/Roadmap.png)

MonoGame has been in development for a long time with a stable outward-facing API based on XNA 4.0, which remains largely static.  The core development has always been the rapid waters behind this API, providing stable and modern access to the native platforms that MonoGame can run on.

The current development branch, targeting a release version of `3.8.5` of MonoGame, is expanding platform support and addressing a key subject of contention, the content editor (MGCB), for which an alternative solution is being devised (the Content Project system).

> [!IMPORTANT]
> All information and dates are subject to change.  In other words, if we can get it done faster, we will!

### [Release 3.8.5](https://github.com/MonoGame/MonoGame/milestone/22) (current development branch)

The `3.8.5` release seeks to formalize the preview of several preview components for testing in preparation for the `3.9` release, namely:

* Preview Vulkan (DesktopVK) platform.
* Preview DX 12 (DesktopDX) platform.
* Preview the new Content Project solution.
* Native Platform evolution (dependencies derived from Platforms instead of dependencies).

> Preview releases are currently available for the `3.8.5` delivery for test.

### [Release 3.8.4](https://github.com/MonoGame/MonoGame/milestone/23) Live Release

The `3.8.4` primary focus was a quick turn-around maintenance release, primarily to address some Android platform issues introduced in 3.8.3.  This release was the first to receive Preview releases (7 in total) to full road test changes, including:

* Improvements to AoT delivery.
* Improvements to Android release delivery.
* New testing pipeline using Samples.
* Upgrade of build dependencies and Wine (for Mac / Linux)
* Compressed texture support in Pipeline.

> See the [Release Log](https://github.com/MonoGame/MonoGame/releases/tag/v3.8.4) for full details.

### [Release 3.9](https://github.com/MonoGame/MonoGame/milestone/17)

`3.9` will effectively be the LTS release of the 3.x series for MonoGame, formalizing the XNA API support and platform access.  It will also include an update to using a more ruggedized and standardized native implementation that is standard across all platforms, making the underlying native elements for MonoGame easier to maintain.

* All the native libraries into their own repo, hook it up to the current system.
* Vulcan support.
* DX 12 support.
* Updated GDK integration for Xbox Consoles (only available to registered ID@Xbox developers)
* New Content Project system.

> [!IMPORTANT]
> Please note that the move to the new native backend will effectively **deprecate the DesktopGL project type**.  This will be replaced with the newer **DesktopVK** platform.
>
> The **WindowsDX** platform is also under review to include support for DX12 and the [Microsoft GDK](https://github.com/microsoft/GDK) (Game Development Kit), once the results are published, the Roadmap will be updated to reflect the plans.

### [Release 4.0](https://github.com/MonoGame/MonoGame/milestone/10) (Provisional)

Version `4.0` of MonoGame aims to break compatibility with the older XNA Content Pipeline system and introduce a much more manageable and extendable system to meet the more modern needs of game development.

### [Release 5.0](https://github.com/MonoGame/MonoGame/milestone/15)

This phase is still very much in the early planning and testing phase, with the ultimate aim to break the older XNA 4.0 API. This will be a very different beast, built from the foundational ideas of XNA / MonoGame.

### Proposed Release Schedule

Just to sneak this in here, we are listening when the community says that we need a faster and more direct delivery of the MonoGame Framework.  To this end, we are proposing to:

* Have regular maintenance updates, ideally delivered through NuGet.
* Annual major releases.

All of this is subject to the will of the community, so if you have an idea or suggestion, make sure to let us know on [Discord](https://discord.gg/monogame).

## MonoGame bounty schedule

> [!NOTE]
> [Also check out the Bounty tracker for the latest](https://github.com/MonoGame/MonoGame/issues/8120)

In order to provide more transparency, the following section details all the current Bounties in production, as well as a provisional list of all other bounties under consideration or development.

> [!IMPORTANT]
> All bounties are subject to the legal requirements set out in the MonoGame Foundation charter.  [Please check the `Bounties` page for more information](https://monogame.net/bounties/)

### Active Bounties

This is a list of all known bounties currently [being worked on](https://github.com/MonoGame/MonoGame/issues/8120):

* [Switch the console runtime from BRUTE to NativeAOT](https://github.com/MonoGame/MonoGame/issues/8194)
* [A better 3D onboarding tutorial](https://github.com/MonoGame/MonoGame/issues/8318)
* [MonoGame 2D advanced series - Shaders](https://github.com/MonoGame/MonoGame/issues/8819)
* [MonoGame 2D advanced series - Mobile targeting](https://github.com/MonoGame/MonoGame/issues/8820)
* [MonoGame 2D advanced series - Networking](https://github.com/MonoGame/MonoGame/issues/8821)

### Bounties in development

* **Advanced onboarding tutorials**

    Working on suggestions for advanced physics integration, complex shaders, animation systems, procedural content generation, optimizations and performance.

* **An Endless runner game sample/starter-kit**

    A reusable sample / starter-kit for developers to build an endless runner game.

* **A first-person 3D game sample/starter-kit**

    A reusable sample / starter-kit for developers to build a first person style game.

* **A third-person 3D game sample/starter-kit**

    A reusable sample / starter-kit for developers to build a third person style game.

* **A network starter sample**

    Building an example framework for building local or networked multi-player games.

* **A graphical/shader starter-kit**

    A quick introduction to effects and shaders in MonoGame.

* **A custom editor sample**

    A starter solution showing how to build an editor for MonoGame showcasing multiple editor windows, multiple viewports (in a single window), custom dialogs, docking windows?, rebuilding content and a simple undo/redo framework (which we may have one available for use).

* **A GUI integration tutorial**
    With so many great GUI systems available for MonoGame, we could do with a tutorial showing how to integrate or build on teh best of them, such as IMGUI, NEX and so on.

* **Texture Compression Content Pipeline**
    The current texture compression systems does not work on `arm` based machines like the Apple M1/M2/M3. This bounty will upgrade the system to use [Basis Universal](https://github.com/BinomialLLC/basis_universal) which works across all supported Desktop plaforms.

> [!NOTE]
> Have an idea for another bounty / sample or something that can make MonoGame even better, or a guide that you feel the community needs, then let us know on GitHub by raising a [Feature Request](https://github.com/MonoGame/MonoGame/issues/new?assignees=&labels=Feature+Request&projects=&template=02_feature_request.yml) and give us the details.

## Onward MonoGame!

Thank you to the MonoGame community for your patience and support, we continue, as ever, to try and make MonoGame the best it can be.
