---
title: MonoGame Roadmap
description: The forecasted roadmap for MonoGame development/improvement, including lists of bounties in development.
_disableToc: true
_disableBreadcrumb: true
---

## MonoGame official roadmap

![Roadmap](./images/Roadmap.png)

MonoGame has been in development for a long time with a stable outward facing API based on XNA 4.0, which remains largely static.  The core development has always been the rapid waters behind this API providing stable and modern access to the native platforms that MonoGame can run on.

The current development branch which is targeting a release version of `3.8.2` of MonoGame is due for release imminently with the main pending item being the improvement of Mac/Linux editor support.

> [!IMPORTANT]
> All information and dates are subject to change.  In other words, if we can get it done faster, we will!

### [Release 3.8.2](https://github.com/MonoGame/MonoGame/milestone/20) (current development branch)

The `3.8.2` is currently in a stabilization phase and its development has been accelerated since the formation of the [MonoGame Foundation](https://monogame.net/about/).  The primary focus is testing and rationalizing the last 2 years of development and improving the Mac/Linux support:

* Update to the .NET 8 Runtime.
* Improved testing and code coverage.
* Update to the XML API documentation, ensuring all of the API is covered. (A Massive task!)
* Improved documentation and migration of original XNA documentation.
* Updated Mac/Linux support.
* Stabilized content pipeline dependencies (moving to open source and linked libraries)

#### Deprecation notices

The **Windows UWP** platform is being retired as this is not a desired publishing platform with Microsoft.  It will be replaced with an updated **DesktopDX** platform that will support current Microsoft deployments in a future release. 

There is a lot of other work going on behind the scenes including **MANY** of the PR's and community submissions to improve the over all framework.  Stay tuned.

### [Release 3.8.3](https://github.com/MonoGame/MonoGame/milestone/21) (Provisional fast track maintenance release)

The primary focus for this release will be to further ratify any outstanding community contributions and reported issues.  Additionally, anything that was left out of 3.8.2 in order to get that release out quicker.

> [!NOTE]
> `3.8.3` will also start the [revised release schedule](#proposed-release-schedule) (subject to community approval and time from the Foundation) aiming to get more regular maintenance releases out with bug fixes.

### [Release 3.9](https://github.com/MonoGame/MonoGame/milestone/17)

`3.9` will effectively be the LTS release of the 3.x series for MonoGame, formalizing the XNA API support and platform access, it will also include an update to using a more ruggedized and standardized native implementation that is standard across all platforms, making the underlying native elements for MonoGame easier to maintain.

* All the native libraries into their own repo, hook it up to the current system.
* Vulcan support.
* DX 12 support.
* Updated GDK integration for Xbox Consoles (only available to registered ID@Xbox developers)

> [!IMPORTANT]
> Please note that the move to the new native backend will effectively **deprecate the DesktopGL project type**, this will be replaced with the newer **DesktopVulkan** platform.
>
> The **WindowsDX** platform is also under review to include support for DX12 and the [Microsoft GDK](https://github.com/microsoft/GDK) (Game Development Kit), once the results ar epublished the Roadmap will be updated to reflect the plans.

### [Release 4.0](https://github.com/MonoGame/MonoGame/milestone/10) (Provisional)

Version `4.0` of MonoGame aims to break compatibility with the older XNA Content Pipeline system and introduce a much more manageable and extendable system to meet the more modern needs of game development.

* New Importer / Processor / Reader / Writer setup.
    One of the main issues with the current setup is that it is one file in, many files out, but it should be many files in many files out, making the current setup is very clunky.
* Replace `.mgcb` files and `mgcb-editor` with a new C# content `project'esk` thingy, a sample from 5 years ago, works well as an [example of what is planned](https://gist.github.com/harry-cpp/76f62c79d96dec9de13f3923fc329784).
* Content compilation will no longer be tied to the building of the project.
    For debugging purposes, a content server will listen for when the game itself request content and build it on the fly.
    When dotnet publish is called on the main game the content project will build all the content only for the current target.

### [Release 5.0](https://github.com/MonoGame/MonoGame/milestone/15)

This phase is still very much in the early planning and testing phase with the ultimate aim to break the older XNA 4.0 API. This will be a very different beast built up on the foundational ideas of XNA / MonoGame.

### Proposed Release Schedule

Just to sneak this in here, we are listening when the community says that we need faster and more direct delivery of the MonoGame Framework, to this end we are proposing to:

* Have regular maintenance updates, ideally delivered through NuGet.
* Annual major releases.

All of this is subject to the will of the community, so if you have an idea or suggestion, make sure to let us know on [Discord](https://discord.gg/monogame)

## MonoGame bounty schedule

> [!NOTE]
> [Also check out the Bounty tracker for the latest](https://github.com/MonoGame/MonoGame/issues/8120)

In order to provide more transparency, the following section details all the current Bounties in production, as well as a provisional list of all other bounties under consideration or development.

> [!IMPORTANT]
> All bounties are subject to the legal requirements set out in the MonoGame Foundation charter, [please check the `Bounties` page for more information](https://monogame.net/bounties/)

### Active Bounties

This is a list of all known bounties currently [being worked on](https://github.com/MonoGame/MonoGame/issues/8120):

* [A shared base for public and console repos](https://github.com/MonoGame/MonoGame/issues/8242)
* [Switch the console runtime from BRUTE to NativeAOT](https://github.com/MonoGame/MonoGame/issues/8194)
* [Implement the Direct3D 12 / GDK/GDKX backend](https://github.com/MonoGame/MonoGame/issues/8195)
* [A better 2D onboarding tutorial](https://github.com/MonoGame/MonoGame/issues/8317)
* [A better 3D onboarding tutorial](https://github.com/MonoGame/MonoGame/issues/8318)

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

* **Upgrade of the [Ship Game](https://github.com/MonoGame/MonoGame.Samples/blob/3.8.1/ShipGame/README.md) sample. (to be more shiny)**

    A bounty to make the Ship Game sample into a more modern showcase sample.

* **A custom editor sample**

    A starter solution showing how to build an editor for MonoGame showcasing multiple editor windows, multiple viewports (in a single window), custom diaglogs, docking windows?, rebuilding content and a simple undo/redo framework (which we may have one available for use).

* **A GUI integration tutorial**
    With so many great GUI systems available for MonoGame, we could do with a tutorial showing how to integrate or build on teh best of them, such as IMGUI, NEX and so on.

* **Texture Compression Content Pipeline**
    The current texture compression systems does not work on `arm` based machines like the Apple M1/M2/M3. This bounty will upgrade the system to use [Basis Universal](https://github.com/BinomialLLC/basis_universal) which works across all supported Desktop plaforms.

> [!NOTE]
> Have an idea for another bounty / sample or something that can make MonoGame even better, or a guide that you feel the community needs, then let us know on GitHub by raising a [Feature Request](https://github.com/MonoGame/MonoGame/issues/new?assignees=&labels=Feature+Request&projects=&template=02_feature_request.yml) and give us the details.

## Onward MonoGame!

Thank you to the MonoGame community for your patience and support, we continue, as ever, to try and make MonoGame the best it can be.
