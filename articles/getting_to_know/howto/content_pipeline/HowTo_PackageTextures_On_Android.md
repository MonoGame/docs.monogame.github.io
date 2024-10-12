---
title: How to package Textures for Android?
description: Describes how to support multiple Texture Compression formats for Android.
requireMSLicense: false
---

The Android ecosystem is unique in that the hardware it runs on can be from many different manufactures.
Unlike iOS or PC you cannot always guarentee what graphics card a user will be using. For this reason
Android needs to have some special attention when shipping your game. 

## Texture Compression

As stated in "[Why use the Content Pipeline](https://docs.monogame.net/articles/getting_started/content_pipeline/why_content_pipeline.html)" you need to be aware of the performance limitations on mobile devices.
The graphics cards on mobile phones do not have the same kind of capabilities as those on the PC or Consoles.
They regularly have less memory and less power. So you need to make use of what you have in a more efficient way.
One of these ways is to use Texture Compression. As stated in "[Why use the Content Pipeline](https://docs.monogame.net/articles/getting_started/content_pipeline/why_content_pipeline.html)" this allows you to fit more
textures on the graphics card than you could if you just used raw RGBA textures.

## How Android deals with textures

Fortunately the Android engineers recognised that supporting all of these texture compresison formats
was not an easy task. So with the introduction of the `.aab` file format they added the ability to 
add multiple texture format files to the package. The way the `.aab` works is that it is not the final
`.apk`. The final `.apk` will be built from the `.aab` when the game is delivered to the end user device. 
As a result not all of the file in the `.aab` will make it to the device. It will filter out things like
`.so` files for other cpu types, and yes, texture formats. 

The `.aab` supports the following directory suffixes for texture compresison

| Texture Format | Suffix |
| -------------- | ------ |
| PVRTC | #tcf_pvrtc |
| ATC | #tcf_atc |
| ASTC| #tcf_astc |
| S3TC | #tcf_s3tc |
| DXT1 | #tcf_dxt1 |
| ETC1 | #tcf_etc1 |
| ETC2 | #tcf_etc2 |

see https://developer.android.com/guide/playcore/asset-delivery/texture-compression

MonoGame has its own [TextureProcessorOutputFormat](https://docs.monogame.net/api/Microsoft.Xna.Framework.Content.Pipeline.Processors.TextureProcessorOutputFormat.html) enumeration which describes the type of Texture Compression 
you use when processing an image. This following table shows you how to map that to the Suffix

| TextureProcessorOutputFormat | Suffix |
| -------------- | ------ |
| PvrCompressed | #tcf_pvrtc |
| AtcCompressed | #tcf_atc |
| AstcCompressed| #tcf_astc |
| DxtCompressed | #tcf_s3tc |
| Etc1Compressed | #tcf_etc1 |
| EtcCompressed | #tcf_etc2 |
| Compressed | No Suffix |

## Adding Texture Compression Suffixes

With the latest MonoGame we added support for being able to have one texture with multiple outputs. 
Previously it would only build the last item, but this has been fixed. 

In the Content Editor 

1. Add a new folder for your desired Suffix. This is usually in the form of `Textures<suffix>`.
2. Right click on the new folder and Add an Existing File.
3. Select the file you want to use for this Suffix and Add it
4. In the Properties of the new file change the TextureFormat to be the one which matches the desired Suffix.

In the `.mgcb` file directly you can do the following

```bash
/processorParam:TextureFormat=PvrCompressed
/build:Textures/LogoOnly_64px.png;Textures#tcf_pvrtc/LogoOnly_64px
```

As documented the [/build](https://docs.monogame.net/articles/getting_started/tools/mgcb.html#build-content-file) command takes 
an optional `<destination_filepath>` after the `<content_filepath>`. We can use this to provide the target folder for our output.
So in the example above, the `LogoOnly_64px.png` file will be compressed using `PvrCompressed` and then the output will end up in `Textures#tcf_pvrtc`.

> !Important
> Some texture formats have specific size requirements. For example PVRTC Compressed Textures MUST be a Power of 2 and Square (e.g 1024x1024).
> Many others need to be Power of 2. It is recommended that you make all your textures Power of 2 


## Sample `.mgcb`

```bash
#begin Textures/LogoOnly_64px.png
/importer:TextureImporter
/processor:TextureProcessor
/processorParam:ColorKeyColor=255,0,255,255
/processorParam:ColorKeyEnabled=True
/processorParam:GenerateMipmaps=False
/processorParam:PremultiplyAlpha=True
/processorParam:ResizeToPowerOfTwo=True
/processorParam:MakeSquare=False
/processorParam:TextureFormat=PvrCompressed
/build:Textures/LogoOnly_64px.png;Textures#tcf_pvrtc/LogoOnly_64px
```

```bash
#----------------------------- Global Properties ----------------------------#

/outputDir:bin/$(Platform)
/intermediateDir:obj/$(Platform)
/platform:Android
/config:
/profile:Reach
/compress:False

#-------------------------------- References --------------------------------#


#---------------------------------- Content ---------------------------------#

#begin ContentFont.spritefont
/importer:FontDescriptionImporter
/processor:FontDescriptionProcessor
/processorParam:PremultiplyAlpha=True
/processorParam:TextureFormat=Compressed
/build:ContentFont.spritefont

#begin Textures/LogoOnly_64px.png
/importer:TextureImporter
/processor:TextureProcessor
/processorParam:ColorKeyColor=255,0,255,255
/processorParam:ColorKeyEnabled=True
/processorParam:GenerateMipmaps=False
/processorParam:PremultiplyAlpha=True
/processorParam:ResizeToPowerOfTwo=True
/processorParam:MakeSquare=False
/processorParam:TextureFormat=PvrCompressed
/build:Textures/LogoOnly_64px.png

#begin Textures/LogoOnly_64px.png
/importer:TextureImporter
/processor:TextureProcessor
/processorParam:ColorKeyColor=255,0,255,255
/processorParam:ColorKeyEnabled=True
/processorParam:GenerateMipmaps=False
/processorParam:PremultiplyAlpha=True
/processorParam:ResizeToPowerOfTwo=True
/processorParam:MakeSquare=False
/processorParam:TextureFormat=PvrCompressed
/build:Textures/LogoOnly_64px.png;Textures#tcf_pvrtc/LogoOnly_64px

#begin Textures/LogoOnly_64px.png
/importer:TextureImporter
/processor:TextureProcessor
/processorParam:ColorKeyColor=255,0,255,255
/processorParam:ColorKeyEnabled=True
/processorParam:GenerateMipmaps=False
/processorParam:PremultiplyAlpha=True
/processorParam:ResizeToPowerOfTwo=True
/processorParam:MakeSquare=False
/processorParam:TextureFormat=DxtCompressed
/build:Textures/LogoOnly_64px.png;Textures#tcf_s3tc/LogoOnly_64px

#begin Textures/LogoOnly_64px.png
/importer:TextureImporter
/processor:TextureProcessor
/processorParam:ColorKeyColor=255,0,255,255
/processorParam:ColorKeyEnabled=True
/processorParam:GenerateMipmaps=False
/processorParam:PremultiplyAlpha=True
/processorParam:ResizeToPowerOfTwo=True
/processorParam:MakeSquare=False
/processorParam:TextureFormat=AtscCompressed
/build:Textures/LogoOnly_64px.png;Textures#tcf_atsc/LogoOnly_64px
```

## Also see

[MGCB File Format](https://docs.monogame.net/articles/getting_started/tools/mgcb.html#build-content-file)
[Why use the Content Pipeline](https://docs.monogame.net/articles/getting_started/content_pipeline/why_content_pipeline.html)
