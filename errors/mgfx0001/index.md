---
title: MonoGame error MGFX0001
description: MGFX0001 error code
---

## Example messages

```
Error: MGFXC0001: MGFXC effect compiler requires a valid Wine installation to be able to compile shaders. Please visit https://docs.monogame.net/errors/mgfx0001?tab=macos for more details.
```

```
Error: MGFXC0001: MGFXC effect compiler requires a valid Wine installation to be able to compile shaders. Please visit https://docs.monogame.net/errors/mgfx0001?tab=linux for more details.
```

## Issue

This error indicates that `Wine` is not installed on the development machine. `Wine` is currently required in order to compile `Effects` on both Linux and MacOS.

## Solution

You need to make sure you have installed `Wine` on your machine. 
Please look at the following [link](https://docs.monogame.net/articles/tutorials/building_2d_games/02_getting_started/?tabs=macos#setup-wine-for-effect-compilation-macos-and-linux-only) for detailed instructions on how to install `Wine` and configure it for use with MonoGame.
