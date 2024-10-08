# How to package Textures on Android

The Android ecosystem is unique in that the hardware it runs on can be from many different manufactures.
Unlike iOS or PC you cannot always guarentee what graphics card a user will be using. For this reason
Android needs to have some special attention when shipping your game. 

## Texture Compression

As stated in [Texture Compress.md] you need to be aware of the performance limitations on mobile devices.
The graphics cards on mobile phones do not have the same kind of capabilities as those on the PC or Consoles.
They regularly have less memory and less power. So you need to make use of what you have in a more efficient way.
One of these ways is to use Texture Compression. As stated in [Texture Compress.md] this allows you to fit more
textures on the graphics card than you could if you just used raw RGBA textures.

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
