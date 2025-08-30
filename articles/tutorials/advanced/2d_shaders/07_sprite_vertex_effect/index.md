---
title: "Chapter 07: Sprite Vertex Shaders"
description: "Learn about vertex shaders and how to use them on sprites"
---

Every shader has two main parts: the pixel shader, which we've been using to change the colors of our sprites, and the vertex shader. The vertex shader runs first, and its job is to determine the final shape and position of our geometry. Up until now, we've been using MonoGame's default vertex shader, which just draws our sprites as flat 2D rectangles.

In this chapter, we're going to unlock the power of the vertex shader. We'll write our own custom vertex shader from scratch, which will allow us to break out of the 2D plane. We'll learn how to use a perspective projection to give our flat world a cool, dynamic 3D feel. 

If you're following along with code, here is the code from the end of the [previous chapter](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/2dShaders/src/06-Color-Swap-Effect).

## Default Vertex Shader

So far in this series, we have only dealt with pixel shaders. To recap, the job of a pixel shader is to convert some input `(u,v)` coordinate into an output color `(r,g,b,a)` value. There has been a second shader function running all along behind the scenes, called the vertex shader. The vertex shader runs _before_ the pixel shader. The job of the vertex shader is to convert world-space vertex data into clip-space vertex data. Technically every call in MonoGame that draws data to the screen must provide a vertex shader function and a pixel shader function. However, the `SpriteBatch` class has a default implementation of the vertex shader that runs automatically.

The default `SpriteBatch` vertex shader takes the vertices that make up the sprites' corners, and applies an _orthographic projection_ to the vertices. The orthographic projection creates a 2d effect where shapes have no perspective, even when they are closer or further away from the origin. 

The vertex shader that is being used can be found [here](https://github.com/MonoGame/MonoGame/blob/develop/MonoGame.Framework/Platform/Graphics/Effect/Resources/SpriteEffect.fx#L29), and is rewritten below,

```hlsl
struct VSOutput
{
	float4 position		: SV_Position;
	float4 color		: COLOR0;
    float2 texCoord		: TEXCOORD0;
};

VSOutput SpriteVertexShader(	float4 position	: POSITION0,
								float4 color	: COLOR0,
								float2 texCoord	: TEXCOORD0)
{
	VSOutput output;
    output.position = mul(position, MatrixTransform);
	output.color = color;
	output.texCoord = texCoord;
	return output;
}
```

The `SpriteVertexShader` looks different from our pixel shaders in a few important ways, 
1. The inputs and outputs are different.
	1. The return type is not just a `float4`, its an entire struct, `VSOutput`,
	2. The inputs are not the same as the pixel shader. The pixel shader got a `Color` and `TextureCoordinates`, but this vertex shader has a `position`, a `color`, and a `texCoord`. 
2. There is a `MatrixTransform` shader parameter available to this shader. 

### Input Semantics

The inputs to the vertex shader mirror the information that the `SpriteBatch` class bundles up for each vertex. If you look at the [SpriteBatchItem](https://github.com/MonoGame/MonoGame/blob/develop/MonoGame.Framework/Graphics/SpriteBatchItem.cs), you will see that each sprite is made up of 4 `VertexPositionColorTexture` instances. 

```csharp
public VertexPositionColorTexture vertexTL;
public VertexPositionColorTexture vertexTR;
public VertexPositionColorTexture vertexBL;
public VertexPositionColorTexture vertexBR;
```

The [`VertexPositionColorTexture`](https://github.com/MonoGame/MonoGame/blob/develop/MonoGame.Framework/Graphics/Vertices/VertexPositionColorTexture.cs) class is a standard MonoGame implementation of the `IVertexType`, and it defines a `Position`, a `Color`, and a `TextureCoordinate` for each vertex. Those should look familiar, because they align with the inputs to the vertex shader function. The alignment is not happenstance, it is enforced by "semantics" that are applied to each field in the vertex. This snippet from the `VertexPositionColorTexture` class defines the semantics for each field in the vertex by specifying the `VertexElementUsage`. 
```csharp

static VertexPositionColorTexture()
{
	var elements = new VertexElement[] 
	{ 
		new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0), 
		new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0), 
		new VertexElement(16, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0) 
	};
	VertexDeclaration = new VertexDeclaration(elements);
}
```

The vertex shader declares a semantic for each input using the `:` syntax. 
```hlsl
// the POSITION0 is the semantic 
float4 position	: POSITION0,
```

>[!warning] 
> You cannot change the `SpriteBatch` vertex shader. 
> 
> The `SpriteBatch` class does not offer any way to change the vertex semantics that are passed to the shader function.   

### Output Semantics

The same concept of semantics applies to the output of the shader. Here is the output type of the vertex shader function. Notice that the fields also have the `:` semantic syntax. These semantics instruct the graphics pipeline how to use the data. 
```hlsl
struct VSOutput
{
	float4 position		: SV_Position;
	float4 color		: COLOR0;
    float2 texCoord		: TEXCOORD0;
};
```

This is the _input_ struct for the standard pixel shaders from previous chapters. Notice how the fields are named slightly differently, but the _semantics_ are identical. 

```hlsl
struct VertexShaderOutput  
{  
   float4 Position : SV_POSITION;  
   float4 Color : COLOR0;  
   float2 TextureCoordinates : TEXCOORD0;  
};
```

### Matrix Transform

The default sprite vertex shader uses this line, 
```hlsl
output.position = mul(position, MatrixTransform);
```

The reason this line exists is to convert the vertices from world-space to clip-space. A vertex is a 3d coordinate in "world-space". But a monitor is a 2d display. Often, the screen's 2d coordinate system is called "clip-space". The vertex shader is converting the 3d world-space coordinate into a 2d clip-space coordinate. That conversion is a vector and matrix multiplication, using the `MatrixTransform`. 

The `MatrixTransform` is computed by the [`SpriteEffect`](https://github.com/MonoGame/MonoGame/blob/develop/MonoGame.Framework/Graphics/Effect/SpriteEffect.cs#L63) class. The relevant lines are copied below,
```csharp
// cache the shader parameter for the MatrixTransform
_matrixParam = Parameters["MatrixTransform"];

// ... some code left out for readability

// create a projection matrix in the _projection variable
Matrix.CreateOrthographicOffCenter(0, vp.Width, vp.Height, 0, 0, -1, out _projection);

// ... some code left out for readability 

// assign the projection matrix to the MatrixTransform
_matrixParam.SetValue(_projection);
```

There are two common types of project matrices, 
1. Orthographic,
2. Perspective

The orthographic projection matrix produces the classic 2d sprite effect, where sprites have no perspective when they are on the sides of the screen. 


## Custom Vertex Shader

Now that you understand the default vertex shader being used by `SpriteBatch`, we can replace the shader with a custom shader. The new shader must accomplish the basic requirements, 
1. convert the vertices from world-space to clip-space
2. provide the input semantics required for the pixel shader.

To experiment with this, create a new Sprite Effect called `3dEffect` in the _MonoGameLibrary_'s shared content effects folder. We need to add a vertex shader function. To do that, we need a new `struct` that holds all the input semantics passed from `SpriteBatch`. 

> [!tip] 
> Use a struct for inputs and outputs.
> 
> The default vertex shader accepts all 3 inputs (`position`, `color`, and `texCoord`) as direct parameters. However, when you have more than 1 semantic, it is helpful to organize all of the inputs in a `struct`. 

```hlsl
struct VertexShaderInput
{
    float4 Position	: POSITION0;
    float4 Color	: COLOR0;
    float2 TexCoord	: TEXCOORD0;
};
```

Now add the stub for the vertex shader function,
```hlsl
VertexShaderOutput MainVS(VertexShaderInput input) 
{
    VertexShaderOutput output;
    
    return output;
}
```

And finally modify the `technique` to _include_ the vertex shader function. Until now, the `MainVS()` function is just considered as any average function in your shader, and since it wasn't used from the `MainPS` pixel shader, it would be compiled out of the shader. When you specify the `MainVS()` function as the vertex shader function, you are overriding the default `SpriteBatch` vertex shader function. 

```hlsl
technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
		VertexShader = compile VS_SHADERMODEL MainVS();
	}
};
```

The shader won't compile yet, because the `VertexShaderOutput` has not been completely initialized. We need to replicate the `MatrixTransform` step to convert the vertices from world-space to clip-space. 

Add the `MatrixTransform` shader parameter.
```hlsl
float4x4 MatrixTransform;
```

And then assign all of the output semantics in the vertex shader.
```hlsl
VertexShaderOutput MainVS(VertexShaderInput input) 
{
    VertexShaderOutput output;
    output.Position = mul(input.Position, MatrixTransform);
    output.Color = input.Color;
    output.TextureCoordinates = input.TexCoord;
    return output;
}
```

To validate this is working, we should try to use the new effect. For now, we will experiment in the `TitleScene`. Create a class member for the new `Material`. 
```csharp
// The 3d material  
private Material _3dMaterial;
```

Load the shader using the hot reload system,
```csharp
// Load the 3d effect 
_3dMaterial = Core.SharedContent.WatchMaterial("effects/3dEffect");
```

And then use the effect when drawing the title text,
```csharp
// Begin the sprite batch to prepare for rendering.  
Core.SpriteBatch.Begin(  
    samplerState: SamplerState.PointClamp,   
    effect: _3dMaterial.Effect);
```

When the game runs, the text will be missing. This is because we never created a projection matrix to assign to the `MatrixTransform` shader parameter. Add this code when loading the material.
```csharp
// Load the 3d effect 
_3dMaterial = Content.WatchMaterial("effects/3dEffect");
_3dMaterial.IsDebugVisible = true;

// this matrix code is taken from the default vertex shader code.
Matrix.CreateOrthographicOffCenter(
	left: 0, 
	right: Core.GraphicsDevice.Viewport.Width, 
	bottom: Core.GraphicsDevice.Viewport.Height, 
	top: 0, 
	zNearPlane: 0, 
	zFarPlane: -1, 
	out var projection);
_3dMaterial.SetParameter("MatrixTransform", projection);
```

And now you should see the text normally again.

![Figure 7.1: The main menu, but rendered with a custom vertex shader](./images/basic.png)

### Making it Move

As a quick experiment, we can show that the vertex shader can indeed modify the vertex positions further if we want to. For now, add a temporary shader parameter called `DebugOffset`, 
```hlsl
float2 DebugOffset;
```

And change the vertex shader to add the `DebugOffset` to the `output.Position` after the clip-space conversion.
```hlsl
output.Position = mul(input.Position, MatrixTransform);  
output.Position.xy += DebugOffset;
```

The sprites now move around as we adjust the shader parameter values. 
![Figure 7.2: We can control the vertex positions](./gifs/basic.gif)

It is important to build intuition for the different coordinate systems involved. Instead of adding the `DebugOffset` _after_ the clip-space conversion, if you try to add it _before_, like in the code below.
```hlsl
float4 pos = input.Position;  
pos.xy += DebugOffset;  
output.Position = mul(position, MatrixTransform);
```

Then you won't see much movement at all. This is because the `DebugOffset` values only go from `0` to `1`, and in world space, this really only amounts to a single pixel. In fact, exactly how much an addition of _`1`_ happens to make is entirely defined _by_ the conversion to clip-space. The `projection` matrix we created treats world space coordinates with an origin around the screen's center, where 1 unit maps to 1 pixel. Sometimes this is exactly what you want, and sometimes it can be confusing. 

![Figure 7.3: Changing coordinates before clip-space conversion](./gifs/basic-2.gif)

### Perspective Projection

The world-space vertices can have their `x` and `y` values modified in the vertex shader, but what about the `z` component? The orthographic projection essentially _ignores_ the `z` component of a vertex and treats all vertices as though they are an equal distance away from the camera. If you change the `z` value, you may _expect_ the sprite to appear closer or further away from the camera, but the orthographic projection matrix does not do that. 

To check, try modify the shader code to adjust the `z` value based on one of the debug values. 
```hlsl
pos.z -= DebugOffset.x;
```

> [!tip] 
> Near and Far plane clipping.
> 
> Keep in mind that if you modify the `z` value _too_ much, it will likely step outside of the near and far planes of the orthographic projection matrix. If this happens, the sprite will vanish, because it the projection matrix doesn't handle coordinates outside of the near and far planes. In the example above, they were defined as `0` and `-1`. 
> ```csharp
> zNearPlane: 0, zFarPlane: -1,
> ```

Nothing happens! 

To fix this, we need to use a _perspective_ projection matrix instead of an orthographic projection matrix. MonoGame has a built in method called `Matrix.CreatePerspectiveFieldOfView()` that will do most of the heavy lifting for us. Once we have a perspective matrix, it would also be helpful to control _where_ the camera is looking. The math is easy, but it would be helpful to put it in a new class. 

Create a new file in the _MonoGameLibrary_'s graphics folder called `SpriteCamera3d`, and paste the following code. We are going to skip over the math internals.
```csharp
using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Graphics;

public class SpriteCamera3d
{
    /// <summary>
    /// The field of view for the camera.
    /// </summary>
    public int Fov { get; set; } = 120;
    
    /// <summary>
    /// By default, the camera is looking at the center of the screen.
    /// This offset value can be used to "turn" the camera from the center towards the given vector value.
    /// </summary>
    public Vector2 LookOffset { get; set; } = Vector2.Zero;

    /// <summary>
    /// Produce a matrix that will transform world-space coordinates into clip-space coordinates.
    /// </summary>
    /// <returns></returns>
    public Matrix CalculateMatrixTransform()
    {
        var viewport = Core.GraphicsDevice.Viewport;
        
        // start by creating the projection matrix
        var projection = Matrix.CreatePerspectiveFieldOfView(
            fieldOfView: MathHelper.ToRadians(Fov),
            aspectRatio: Core.GraphicsDevice.Viewport.AspectRatio,
            nearPlaneDistance: 0.0001f,
            farPlaneDistance: 10000f
        );

        // position the camera far enough away to see the entire contents of the screen
        var cameraZ = (viewport.Height * 0.5f) / (float)Math.Tan(MathHelper.ToRadians(Fov) * 0.5f);
        
        // create a view that is centered on the screen
        var center = .5f * new Vector2(viewport.Width, viewport.Height);
        var look = center + LookOffset;
        var view = Matrix.CreateLookAt(
            cameraPosition: new Vector3(center.X, center.Y, -cameraZ),    
            cameraTarget: new Vector3(look.X, look.Y, 0),
            cameraUpVector: Vector3.Down
        );

        // the standard matrix format is world*view*projection, 
        //  but given that we are skipping the world matrix, its just view*projection
        return view * projection;
    }
}
```

And now instead of creating an orthographic matrix in the `TitleScene`, we can use the new class.
```csharp
// Load the 3d effect 
_3dMaterial = Core.SharedContent.WatchMaterial("effects/3dEffect");
_3dMaterial.IsDebugVisible = true;

var camera = new SpriteCamera3d();
_3dMaterial.SetParameter("MatrixTransform", camera.CalculateMatrixTransform());
```

Moving the `z` value uniformly in the shader won't be visually stimulating. A more impressive demonstration of the _perspective_ projection would be to rotate the vertices around the center of the sprite. 

```hlsl

VertexShaderOutput MainVS(VertexShaderInput input) 
{
    VertexShaderOutput output;

    float4 pos = input.Position;
    
    // hardcode the screen-size for now. 
    float2 screenSize = float2(1280, 720);
    
    // create the center of rotation
    float2 centerXZ = float2(screenSize.x * .5, 0);
    
    // convert the debug variable into an angle from 0 to 2 pi. 
    //  shaders use radians for angles, so 2 pi = 360 degrees
    float angle = DebugOffset.x * 6.28;
    
    // pre-compute the cos and sin of the angle
    float cosA = cos(angle);
    float sinA = sin(angle);
    
    // shift the position to the center of rotation
    pos.xz -= centerXZ;
    
    // compute the rotation
    float nextX = pos.x * cosA - pos.z * sinA;
    float nextZ = pos.x * sinA + pos.z * cosA;
    
    // apply the rotation
    pos.x = nextX;
    pos.z = nextZ;
    
    // shift the position away from the center of rotation
    pos.xz += centerXZ;
    
    
    output.Position = mul(pos, MatrixTransform);
    output.Position /= output.Position.w; // re-normalize the position vector.
    output.Color = input.Color;
    output.TextureCoordinates = input.TexCoord;
    return output;
}
```

> [!note]
> What does this do, `output.Position /= output.Position.w` ? 
> Long story short, the `w` component of the `.Position` must be _1_. Dividing any number by itself results in _1_, so the dividing `output.Position` by its own `w` component does two things, 
>  1. sets the `w` component to _1_, 
>  2. uniformly adjusts the other components to accomodate the change.
>     
> The math to fully explain why this is required is beyond the scope of this tutorial series. Read about [homogenous coordinates](https://www.tomdalling.com/blog/modern-opengl/explaining-homogenous-coordinates-and-projective-geometry/) and the [perspective divide](https://www.scratchapixel.com/lessons/3d-basic-rendering/perspective-and-orthographic-projection-matrix/projection-matrix-GPU-rendering-pipeline-clipping.html)


And now when the debug parameter is adjusted, the text spins in a way that was never possible with the default `SpriteBatch` vertex shader.

![Figure 7.4: A spinning text](./gifs/spin-1.gif)

The text disappears for half of the rotation. That happens because as the vertices are rotated, the triangle itself started to point _away_ from the camera. By default, `SpriteBatch` will cull any faces that point away from the camera. Change the `rasterizerState` to `CullNone` when beginning the sprite batch.

```csharp
// Begin the sprite batch to prepare for rendering.  
Core.SpriteBatch.Begin(  
    samplerState: SamplerState.PointClamp,   
    rasterizerState: RasterizerState.CullNone,  
    effect: _3dMaterial.Effect);
```

And voilà, the text no longer disappears on its flip side. 

![Figure 7.4: A spinning text with reverse sides](./gifs/spin-2.gif)

You may find that the field of view is too high for your taste. Try lowering the field of view to 60, and you'll see something similar to this,
![Figure 7.5: A spinning text with reverse sides with smaller fov](./gifs/spin-3.gif)

As a final touch, we should remove the hard-coded `screenSize` variable from the shader, and extract it as a shader parameter. While we are at it, clean up and remove the debug parameters as well.
```hlsl
float2 ScreenSize;
float SpinAngle;
```

Then, make sure to set the ScreenSize parameter correctly from C#,
```csharp
_3dMaterial.SetParameter("ScreenSize", new Vector2(Core.GraphicsDevice.Viewport.Width, Core.GraphicsDevice.Viewport.Height));
```

And instead of manually controlling the spin angle, we can make the title spin gentle following the mouse position. In the `Update()` function the `TitleScreen`, add the following snippet,

```csharp
var spinAmount = Core.Input.Mouse.X / (float)Core.GraphicsDevice.Viewport.Width;  
spinAmount = MathHelper.SmoothStep(-.1f, .1f, spinAmount);  
_3dMaterial.SetParameter("SpinAmount", spinAmount);
```

![Figure 7.6: Spin controlled by the mouse](./gifs/spin-4.gif)

## Applying it to the Game

It was helpful to use the `TitleScene` to build intuition for the vertex shader, but now it is time to apply the perspective vertex shader to the game itself to add immersion and a sense of depth to the gameplay. The goal is to use the same effect in the `GameScene`. 
### The Uber Shader

A problem emerges right away. The `GameScene` is already using the color swapping effect to draw the sprites, and `SpriteBatch` can only use a single per batch. 

To solve this problem, we will collapse our shaders into a single shader that does all _both_ the color swapping _and_ the vertex manipulation. Writing code to be re-usable is a challenge for all programming languages, and shader language is no different. Sometimes when a game has lots of different effects collapsed into a single shader, the shader is called the _Uber Shader_. For _Dungeon Slime_, that term is premature, but the spirit is the same. 

MonoGame shaders can reference code from multiple files by using the `#include` syntax. MonoGame itself [uses](https://github.com/MonoGame/MonoGame/blob/develop/MonoGame.Framework/Platform/Graphics/Effect/Resources/SpriteEffect.fx#L8) this technique itself in the default vertex shader for `SpriteBatch`. We can move some of the code from our existing `.fx` files into a _new_ `.fxh` file, re-write the existing shaders to `#include` the new `.fxh` file, and then be able to write additional `.fx` files that `#include` multiple of our files and compose the functions into a single effect. 

> [!tip] 
> `.fxh` vs `.fx`.
> 
> `.fxh` is purely convention. Technically you can use whatever file extension you want, but `.fxh` implies the usage of the file is for shared code, and does not contain a standalone effect itself. The `h` references `header`. 

Before we get started, we are going to be editing `.fxh` files, so it would be nice if the hot-reload system also listened to these `.fxh` file changes. Update the `Watch` configuration in the `DungeonSlime.csproj` file to include the `.fxh` file type.
```xml
  <ItemGroup Condition="'$(OnlyWatchContentFiles)'=='true'">
    <!-- Adds .fx files to the `dotnet watch`'s file scope -->
    <Watch Include="../**/*.fx;../**/*.fxh" />
    
    <!-- Removes the .cs files from `dotnet watch`'s compile scope -->
    <Compile Update="**/*.cs" Watch="false" />
  </ItemGroup>

```

Let's start by factoring out some shared components a few different `.fxh` files. 

Create a file in the _MonoGameLibrary_'s shared effect content folder called `common.fxh`. This file will contain utilities that can be shared for all effects, such as the `struct` types that define the inputs and outputs of the vertex and pixel shaders.

```hlsl
#ifndef COMMON
#define COMMON 1

struct VertexShaderInput
{
    float4 Position	: POSITION0;
    float4 Color	: COLOR0;
    float2 TexCoord	: TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};
#endif
```

>[!tip] 
> Include Guards.
> 
> The `#include` syntax is taking the referenced file and inserting it into the code. If the same file was included twice, then the contents that file would be written out as code _twice_. Defining a `struct` or function this way would cause the compiler to fail, because the `struct` would be declared twice, which is illegal. To work around this, _a_ solution is to use a practice called "include guards", where the file itself defines a symbol (in the case above, the symbol is `COMMON`). The file only compiles to anything if the symbol has not yet been defined. The `#ifndef` stands for "if not yet defined". Once the `COMMON` symbol is defined once, any future inclusions of the file won't match the `#ifndef` clause. 

Then, in the `3dEffect.fx` file, remove the `VertexShaderInput` and `VertexShaderOutput` structs and replace them with this line,
```hlsl
#include "common.fxh"
```

If you run the game, nothing should change, except that the shader code is more modular. To continue, create another file next to `3dEffect.fx` called `3dEffect.fxh`. Paste the contents, 
```hlsl
#ifndef EFFECT_3DEFFECT
#define EFFECT_3DEFFECT
#include "common.fxh"

float4x4 MatrixTransform;  
float2 ScreenSize;  
float SpinAmount;

VertexShaderOutput MainVS(VertexShaderInput input) 
{
    VertexShaderOutput output;

    float4 pos = input.Position;
    
    // create the center of rotation
    float2 centerXZ = float2(ScreenSize.x * .5, 0);
    
    // convert the debug variable into an angle from 0 to 2 pi. 
    //  shaders use radians for angles, so 2 pi = 360 degrees
    float angle = SpinAmount * 6.28;
    
    // pre-compute the cos and sin of the angle
    float cosA = cos(angle);
    float sinA = sin(angle);
    
    // shift the position to the center of rotation
    pos.xz -= centerXZ;
    
    // compute the rotation
    float nextX = pos.x * cosA - pos.z * sinA;
    float nextZ = pos.x * sinA + pos.z * cosA;
    
    // apply the rotation
    pos.x = nextX;
    pos.z = nextZ;
    
    // shift the position away from the center of rotation
    pos.xz += centerXZ;
    
    output.Position = mul(pos, MatrixTransform);
    output.Position /= output.Position.w; // re-normalize the position vector.
    output.Color = input.Color;
    output.TextureCoordinates = input.TexCoord;
    return output;
}
#endif
```

And now in the `3dEffect.fx`, instead of `#include` referencing the `common.fxh`, we can directly reference `3dEffect.fxh`. We should also remove the code that was just pasted into the new common header file. Here is the entire contents of the slimmed down `3dEffect.fx` file,
```hlsl
#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

#include "3dEffect.fxh"

float4 MainPS(VertexShaderOutput input) : COLOR
{
	return tex2D(SpriteTextureSampler,input.TextureCoordinates) * input.Color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
		VertexShader = compile VS_SHADERMODEL MainVS();
	}
};
```

It is time to do the same thing for the `colorSwapEffect.fx` file. The goal is to split the file apart into a header file that defines the components of the effect, and leave the `fx` file itself without much _implementation_. Create a new file called `colors.fxh`, and paste the following.

```hlsl
#ifndef COLORS
#define COLORS

#include "common.fxh"

// the custom color map passed to the Material.SetParameter()
Texture2D ColorMap;
sampler2D ColorMapSampler = sampler_state
{
	Texture = <ColorMap>;
	MinFilter = Point;
    MagFilter = Point;
    MipFilter = Point;
    AddressU = Clamp;
    AddressV = Clamp;
};

// a control variable to lerp between original color and swapped color
float OriginalAmount;
float Saturation;

float4 Grayscale(float4 color)
{
    // Calculate the grayscale value based on human perception of colors
    float grayscale = dot(color.rgb, float3(0.3, 0.59, 0.11));

    // create a grayscale color vector (same value for R, G, and B)
    float3 grayscaleColor = float3(grayscale, grayscale, grayscale);

    // Linear interpolation between he grayscale color and the original color's
    // rgb values based on the saturation parameter.
    float3 finalColor = lerp(grayscale, color.rgb, Saturation);

    // Return the final color with the original alpha value
    return float4(finalColor, color.a);
}

float4 SwapColors(float4 color)
{
	// produce the key location
	//  note the x-offset by half a texel solves rounding errors.
	float2 keyUv = float2(color.r , 0);
	
	// read the swap color value
	float4 swappedColor = tex2D(ColorMapSampler, keyUv) * color.a;
	
	// ignore the swap if the map does not have a value
	bool hasSwapColor = swappedColor.a > 0;
	if (!hasSwapColor)
	{
	    return color;
	}
	
	// return the result color
	return lerp(swappedColor, color, OriginalAmount);
}

float4 ColorSwapPS(VertexShaderOutput input) : COLOR
{
    // read the original color value
	float4 originalColor = tex2D(SpriteTextureSampler,input.TextureCoordinates);

    float4 swapped = SwapColors(originalColor);
    float4 saturated = Grayscale(swapped);
    
    return saturated;
}

#endif
```

Then, then `colorSwapEffect.fx` file can be re-written as this code,
```hlsl
#if OPENGL  
   #define SV_POSITION POSITION  
   #define VS_SHADERMODEL vs_3_0  
   #define PS_SHADERMODEL ps_3_0  
#else  
   #define VS_SHADERMODEL vs_4_0_level_9_1  
   #define PS_SHADERMODEL ps_4_0_level_9_1  
#endif  
  
// the main Sprite texture passed to SpriteBatch.Draw()  
Texture2D SpriteTexture;  
sampler2D SpriteTextureSampler = sampler_state  
{  
   Texture = <SpriteTexture>;  
};  
  
#include "colors.fxh"  
  
technique SpriteDrawing  
{  
   pass P0  
   {  
      PixelShader = compile PS_SHADERMODEL ColorSwapPS();  
   }  
};
```



Now most of the components we'd like to combine into a single effect have been split into various `.fxh` header files. Create a new sprite effect in the _DungeonSlime_'s content effect folder called `gameEffect.fx`. To start, if you try to `#include "common.fxh"` , you will see an error like this, 
```
error PREPROCESS01: File not found: common.fxh in .(MonoGame.Effect.Preprocessor+MGFile)
```

This happens because the `gameEffect.fx` file is in a different folder than the `common.fxh` file, and the `"common.fxh"`  is treated as a relative _file path_ lookup. Instead, in the `gameEffect.fx` file, use this line, 
```hlsl
#include "../../../MonoGameLibrary/SharedContent/effects/common.fxh"
```

Then, the `gameEffect.fx` file could also reference the other two `.fxh` files we created,
```hlsl
#include "../../../MonoGameLibrary/SharedContent/effects/3dEffect.fxh"
#include "../../../MonoGameLibrary/SharedContent/effects/colors.fxh"
```

And the only thing the `gameEffect.fx` file needs to specify is which functions to use for the vertex shader and pixel shader functions. 
```hlsl
technique SpriteDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL ColorSwapPS();
	}
};
```

The entire contents of the `gameEffect.fx` are written below.
```hlsl
#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

#include "../../../MonoGameLibrary/SharedContent/effects/3dEffect.fxh"
#include "../../../MonoGameLibrary/SharedContent/effects/colors.fxh"

technique SpriteDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL ColorSwapPS();
	}
};
```

To load it into the `GameScene`, we need to _delete_ the old class member for `_colorSwapMaterial`, and add a new one,
```csharp
// The uber material for the game objects  
private Material _gameMaterial;
```

And then apply all of the parameters to the single material,
```csharp
// Load the game material
_gameMaterial = Content.WatchMaterial("effects/gameEffect");
_gameMaterial.IsDebugVisible = true;
_gameMaterial.SetParameter("ColorMap", _colorMap);
var camera = new SpriteCamera3d();
_gameMaterial.SetParameter("MatrixTransform", camera.CalculateMatrixTransform());
_gameMaterial.SetParameter("ScreenSize", new Vector2(Core.GraphicsDevice.Viewport.Width, Core.GraphicsDevice.Viewport.Height));
```

Any place where the old `_colorSwapMaterial` is being referenced should be changed to use the `_gameMaterial` instead. Now, if you run the game, the color swap controls are still visible, but we can also manually control the tilt of the map.

![Figure 7.7: All of the effects in one](./gifs/uber.gif)

### Adjusting the Game

Now that the 3d effect can be applied to the game objects, it would be good to make the world tilt slightly towards the player character to give the movement more weight. Instead of spinning the entire map, an easier approach will be to modify the `MatrixTransform` that is being passed to the shader. 

Add this snippet to the top of the `GameScene`'s `Update()` method.

```csharp
// Set the camera view to look at the player slime
var viewport = Core.GraphicsDevice.Viewport;
var center = .5f * new Vector2(viewport.Width, viewport.Height);
var slimePosition = new Vector2(_slime?.GetBounds().X ?? center.X, _slime?.GetBounds().Y ?? center.Y);
var offset = .01f * (slimePosition - center);
_camera.LookOffset = offset;
_gameMaterial.SetParameter("MatrixTransform", _camera.CalculateMatrixTransform());
```

![Figure 7.8: Camera follows the slime](./gifs/cam-follow.gif)

The clear color of the scene can be seen in the corners (the `CornflowerBlue`). Pick whatever clear color you think looks good for the color swapping. 
```csharp
Core.GraphicsDevice.Clear(new Color(32, 16, 20));
```

And to finish this chapter, the game looks like this,
![Figure 7.9: vertex shaders make it pop](./gifs/final.gif)

## Conclusion

Our game has a whole new dimension! In this chapter, you accomplished the following:

- Learned the difference between a vertex shader and a pixel shader.
- Wrote a custom vertex shader to override the `SpriteBatch` default.
- Replaced the default orthographic projection with a perspective projection to create a 3D effect.
- Refactored shader logic into modular `.fxh` header files for better organization.
- Combined vertex and pixel shader effects into a single "uber shader".

The world feels much more alive now that it tilts and moves with the player. In the next chapter, we'll build on this sense of depth by tackling a 2D dynamic lighting system.

You can find the complete code sample for this chapter, [here](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/2dShaders/src/07-Sprite-Vertex-Effect). 

Continue to the next chapter, [Chapter 08: Light Effect](../08_light_effect/index.md)