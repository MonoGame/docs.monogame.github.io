---
title: How To Extend a Standard Content Processor?
description: Describes how MonoGame lets you modify or extend the behavior of any standard Content Pipeline processor that ships with the product.
requireMSLicense: true
---

MonoGame lets you modify or extend the behavior of any standard Content Pipeline processor that ships with the product. See [Standard Content Importers and Content Processors](../../whatis/Content_Pipeline/CP_StdImpsProcs.md) for a description of standard processors.

Because there are so many asset variants supported by different digital content creation (DCC) tools, it is often useful to be able to modify how one of the standard processors operates. The following examples illustrate some of the kinds of things you might want to do.

> **Tip**
>
> The following code samples are provided only for demonstration. Most of the functionality described is already available by using parameters on a standard processor.

## Adding a Scaling Operation to a Processor

There are many reasons why you might want to modify the existing functionality of a standard processor. Here is one example. If your source assets and your game are at different scales, you may want the processor to scale each model automatically at build time. You can implement such automatic scaling by overriding the [Process](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelProcessor) method of the [ModelProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelProcessor) class, which generates a [Model](xref:Microsoft.Xna.Framework.Graphics.Model). In the override, you first scale the entire scene, and then invoke the base class functionality to process as usual.

The following code illustrates this technique:

```csharp
    [ContentProcessor]
    class ScalingModelProcessor : ModelProcessor
    {
        public override ModelContent Process(
            NodeContent input, ContentProcessorContext context )
        {
            MeshHelper.TransformScene( input, Matrix.CreateScale( 10.0f ) );
            return base.Process( input, context );
        }
    }
```

## Generating Additional Data

In some cases, you may want to add information to a game asset that a standard processor would not. For example, if a custom effect you want to apply requires tangent or binormal data, you can extend the standard model processor to build this additional data into the asset. To do this, you override the [Process](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelProcessor) method of the [ModelProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelProcessor) class. In the override, navigate the [NodeContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.NodeContent) hierarchy of the game asset, and call [CalculateTangentFrames](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.MeshHelper) for each [MeshContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.MeshContent) object you find.

The following code shows how to do this:

```csharp
    [ContentProcessor]
    class ModelProcessorWithTangents : ModelProcessor
    {
        public override ModelContent Process( NodeContent input, ContentProcessorContext context )
        {
            GenerateTangentFramesRecursive( input );
            return base.Process( input, context );
        }

        private void GenerateTangentFramesRecursive( NodeContent node )
        {
            MeshContent mesh = node as MeshContent;
            if (mesh != null)
            {
                MeshHelper.CalculateTangentFrames( mesh, VertexChannelNames.TextureCoordinate( 0 ), 
                    VertexChannelNames.Tangent( 0 ), VertexChannelNames.Binormal( 0 ) );
            }

            foreach (NodeContent child in node.Children)
            {
                GenerateTangentFramesRecursive( child );
            }
        }
    }
```

## Changing the Processors Called for Child Objects

Another useful technique is to override a standard processor, and to change how child objects are processed just by changing the processors they use.

Consider, for example, the hierarchy of calls through which textures in a model are processed:

- The standard [ModelProcessor.Process](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelProcessor) method is called to process a [NodeContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.NodeContent) object that represents the root of a scene.
- `ModelProcessor.Process` in turn calls the [ModelProcessor.ConvertMaterial](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelProcessor) method once for every [MaterialContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.MaterialContent) object used in the scene.
- `ModelProcessor.ConvertMaterial` in turn invokes the [MaterialProcessor.Process](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.MaterialProcessor) method on the [MaterialContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.MaterialContent) object passed to it.
- `MaterialProcessor.Process` in turn calls the [MaterialProcessor.BuildTexture](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.MaterialProcessor) method once for each texture in the [MaterialContent.Textures](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.MaterialContent.Textures) collection in the `MaterialContent` object passed to it.
- `MaterialProcessor.BuildTexture` in turn invokes the [ModelTextureProcessor.Process](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.TextureProcessor) method on the [TextureContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.TextureContent) object passed to it.

One reason you may want to change how this works is that the `ModelTextureProcessor.Process` method applies texture compression to all textures it processes. This could be DXT1, DXT5, PVRTC, ETC1, RGBA4444 or ATITC depending on target your platform. If textures in your game assets are compressed already, you may want to avoid a second compression.

> **Tip**
>
> Not all platforms support all types of texture compression. For example DXT1/5 are generally only supported on Desktop graphics cards and some NVidia mobile graphics cards. PVRTC is only supported on iOS and some Android devices with PowerVR graphics cards, and ATITC is only supported on ATI graphics cards. Using the `Compressed` setting for `TextureCompression` for the Texture Processor will let the Pipeline pick the best compression for your target platform.

### To prevent compression of textures during processing

Here is how to prevent compression from being applied to model textures during processing.

1. Create an override of the standard [MaterialProcessor.BuildTexture](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.MaterialProcessor) method, and invoke the [TextureProcessor.Process](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.TextureProcessor) method, which does no compression, instead of `ModelTextureProcessor.Process`.
2. Create an override of `ModelProcessor.ConvertMaterial` that invokes your override of `MaterialProcessor.BuildTexture` instead of the standard one.

The first of these overrides could be coded as:

```csharp
    [ContentProcessor]
    class NoCompressionMaterialProcessor : MaterialProcessor
    {
        protected override ExternalReference<TextureContent> BuildTexture( 
            string textureName, ExternalReference<TextureContent> texture, ContentProcessorContext context )
        {
            return context.BuildAsset<TextureContent, TextureContent>( texture, "TextureProcessor" );
        }
    }
```

There are several things to note about this code.

- An [ExternalReference](xref:Microsoft.Xna.Framework.Content.Pipeline) is an asset object that is shared between multiple classes, such as a diffuse texture used by more than one material. When such an asset is specified, the Content Manager loads only one copy of the `ExternalReference` at run time and builds it only once, no matter how many references there are to it.
- The [ContentProcessorContext](xref:Microsoft.Xna.Framework.Content.Pipeline.ContentProcessorContext)[BuildAsset](xref:Microsoft.Xna.Framework.Content.Pipeline.ContentProcessorContext) method lets you invoke a processor by name to build the content in an object.
- Although _textureName_, the first argument to `BuildTexture`, is ignored in the override above, you could use it if you wanted to process textures differently depending on normal maps or other criteria.

Given the processor created by your first override above, you could code the second override:

```csharp
    [ContentProcessor]
    class NoCompressionModelProcessor : ModelProcessor
    {
        protected override MaterialContent ConvertMaterial(
            MaterialContent material, ContentProcessorContext context )
        {
            return context.Convert<MaterialContent, MaterialContent>(
                material, "NoCompressionMaterialProcessor" );
        }
    }
```

Because this override is processing `MaterialContent` objects in memory rather than `ExternalReference` objects, it uses the [ContentProcessorContext.Convert](xref:Microsoft.Xna.Framework.Content.Pipeline.ContentProcessorContext) function instead of [BuildAsset](xref:Microsoft.Xna.Framework.Content.Pipeline.ContentProcessorContext) to invoke the processor created by your first override.

After building and installing your new `NoCompressionModelProcessor` (see [Adding a Custom Importer](../../whatis/Content_Pipeline/CP_AddCustomProcImp.md)), you can assign it to any models whose textures are already compressed and no further compression is applied.

## See Also

- [Adding New Content Types](../../whatis/Content_Pipeline/CP_Content_Advanced.md)  
- [What Is Content?](../../whatis/Content_Pipeline/CP_Overview.md)  
- [What is the Content Pipeline?](../../whatis/Content_Pipeline/CP_Architecture.md)  
- [Standard Content Importers and Content Processors](../../whatis/Content_Pipeline/CP_StdImpsProcs.md)  
- [Adding a Custom Importer](../../whatis/Content_Pipeline/CP_AddCustomProcImp.md)  
