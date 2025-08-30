---
title: "Chapter 10: Next Steps"
description: "Review your accomplishments and consider next steps"
---

Congratulations on making it to the end of the advanced 2D shaders tutorial! We have come an incredibly long way from our starting point. Let's take a moment to look back at everything we've accomplished.

We started by building a robust **hot-reloading workflow** to make shader development fast and interactive. We then built a `Material` class and a real-time **debug UI** to make our shader code safer and easier to work with.

With that foundation, we dove into creating effects. We built:
- A flexible, texture-driven **screen transition effect**.
- A powerful **color-swapping system** using a Look-Up Table (LUT).
- A **3D perspective effect** by writing our first custom vertex shader.
- A complete **2D lighting and shadow system** using advanced techniques like deferred rendering, normal mapping, and the stencil buffer.

You have the tools to develop shaders in MonoGame, and you have implemented a set of effects that bring 2D games to life. 

## Next Steps

The world of graphics programming is vast and there is always more to learn. If you're excited and want to keep going, here are a few topics to explore.

#### Signed Distance Fields (SDFs)
Signed Distance Fields are a powerful technique for rendering resolution-independent shapes, text, and UI elements. Instead of a texture storing color, it stores the distance to the nearest edge of a shape. With some shader code, you can render perfectly crisp shapes at any size. They are fantastic for fonts and for creating procedural effects like outlines, glows, and soft shadows.
#### Advanced Post-Processing
We touched on post-processing with our scene transition, but there's a whole world of effects you can apply to the final rendered image. Effects like bloom, depth of field, chromatic aberration, and film grain can all be implemented as shaders that process the entire screen to give your game a unique stylistic look. Check out the bloom effect in the [NeonShooter](https://github.com/MonoGame/MonoGame.Samples/blob/3.8.4/NeonShooter/NeonShooter.Core/Content/Shaders/BloomCombine.fx) sample.
#### Beyond SpriteBatch
In this tutorial, we worked within the confines of the default `SpriteBatch` vertex format (`VertexPositionColorTexture`). MonoGame can draw arbitrary vertex buffer data with the `GraphicsDevice.DrawPrimitives()` functions. You can draw shapes with more than 4 corners. You can pass extra data _per_ vertex, like a unique id, custom data, or whatever you need for your effects. If you want to get into 3d game development, then you'll need to expand beyond `SpriteBatch`. 

## Continued Reading

This tutorial series was an exploration of various shader topics, with a focus on MonoGame's tooling. As you continue to develop new effects and shaders for your games, you will undoubtedly need to research far and wide on the internet for help. Graphics code can be notoriously hard. Here are a few resources that may help you. 

- [HLSL Intrinsic](https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-intrinsic-functions) lists out the functions you can use in HLSL.
- [The Book Of Shaders](https://thebookofshaders.com/) goes from shader fundamentals to building complicated effects. It has live code editors in the browser so you can tweak the shader and see the result right away!
- [Shadertoy](https://www.shadertoy.com/) is an online shader editor with a live preview in the browser. It also functions as a vibrant library of cool effects and techniques you can learn from. Often, if there is a technique you want to learn, some wizard has implemented it on Shadertoy.
- [Inigo Quilez](https://iquilezles.org/) is a legend in the shader community and has written _many_ fantastic tutorials. 
- There are a lot of wonderful Youtube videos that cover shaders. Here are just a few.
    - [Dan Moran](https://www.youtube.com/playlist?list=PLJ4rOFLQFH4BUVziWikfHvL8TbNGJ6M_f) makes the _Makin' Stuff Look Good In Video Games_. This channel has videos examining effects in existing games and recreating them by hand. 
    - [The Art Of Code](https://www.youtube.com/playlist?list=PLGmrMu-IwbguU_nY2egTFmlg691DN7uE5) has several videos writing shaders from scratch with Shadertoy. 
    - [Acerola](https://www.youtube.com/playlist?list=PLUKV95Q13e_U5g00d5M5MOacpVMiEbW9u) has great case study videos recreating effects from existing games.
    - [Freya Holmér](https://www.youtube.com/watch?v=MOYiVLEnhrw) has many great videos that delve into mathematics for game developers. 


## A Note From The Author
Hey friend, 
If you read through this whole series, then _thank you_ for lending me your time and patience. Hopefully you learned some good information. I remember it took me at least 3 distinct attempts to learn shaders. Once when I was a teenager. Once when I was in college. And then finally again as a eager-eyed young developer in the software world. Each attempt got me further, but it took me _decades_ to understand anything at all. If you find shaders confusing, please don't give up. Keep going. 

Best,
-[Chris Hanna](https://github.com/cdhanna)