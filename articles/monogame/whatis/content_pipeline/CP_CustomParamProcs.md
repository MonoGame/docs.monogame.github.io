---
title: What are Parameterized Processors?
description: This topic discusses a method for programmatically modifying existing parameter values, and for adding new parameters to your own processors.
---

# Developing with Parameterized Processors

This topic discusses a method for programmatically modifying existing parameter values, and for adding new parameters to your own processors.

## Programmatically Setting Parameter Values

When you need to pass parameter values from one processor to another (also referred to as chaining), use the [BuildAsset](xref:Microsoft.Xna.Framework.Content.Pipeline.ContentProcessorContext) and [BuildAndLoadAsset](xref:Microsoft.Xna.Framework.Content.Pipeline.ContentProcessorContext) methods. Pass the parameter and its value using the _processorParameters_ argument of the respective method. For example, a custom model processor would invoke a second processor for model textures with a call to [BuildAsset](xref:Microsoft.Xna.Framework.Content.Pipeline.ContentProcessorContext) and pass any parameter values in the _processorParameters_ argument.

The following code example demonstrates this technique. First, add several parameters to a data dictionary:

```csharp
      //create a dictionary to hold the processor parameter
      OpaqueDataDictionary parameters = new OpaqueDataDictionary();

      //add several parameters to the dictionary
      parameters.Add( "ColorKeyColor", Color.Magenta );
      parameters.Add( "ColorKeyEnabled", true );
      parameters.Add( "ResizeToPowerOfTwo", true );
```

After adding the necessary parameters, pass the dictionary to the chained processor:

```csharp
      context.BuildAsset<TextureContent, TextureContent="">(
      texture, typeof( TextureProcessor ).Name,
      parameters,
      null,
      null );
```

This call passes all parameters (stored in `parameters`) to a texture processor.

Again, any parameters not recognized by the receiving processor are ignored. Therefore, if the parameter `ColorKeyCode` is entered into the dictionary as _ColourKeyCode_, it is ignored by the receiving processor.

## Declaring Process Parameters

Adding one or more parameters to your custom processor requires additional code in your processor's definition. Parameters support the following types:

- bool
- byte
- sbyte
- char
- decimal
- double
- float
- int
- uint
- long
- ulong
- short
- ushort
- string
- enum
- [Vector2](xref:Microsoft.Xna.Framework.Vector2), [Vector3](xref:Microsoft.Xna.Framework.Vector3), and [Vector4](xref:Microsoft.Xna.Framework.Vector4)
- [Color](xref:Microsoft.Xna.Framework.Color)

Parameters of other types are ignored by the processor.

> **Tip**
>
> Apply the Browsable attribute (with a value of **false**) to an individual parameter to prevent that parameter from being displayed in the Properties window.

The following code example defines a simple custom processor that switches the coordinate system of a model using a single parameter (called switchCoordinateSystem):

```csharp
      public class SwitchCoordSystemProcessor : ModelProcessor
      {
      #region Processor Parameters
      private bool switchCoordinateSystem = false;

      [DisplayName("Switch Coordinate System")]
      [DefaultValue(false)]
      [Description("Switches the coordinate system of a model.")]
      public bool SwitchCoordinateSystem
      {
        get { return switchCoordinateSystem; }
        set { switchCoordinateSystem = value; }
      }
      //additional class code follows...
```

In this code, the `SwitchCoordSystemProcessor` class is derived from [ModelProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelProcessor). This indicates that the processor accepts a model as input. The next few lines declare a single property called `SwitchCoordinateSystem` of type **bool**. Note that every parameter must have a **set*- method. The property also has several attributes applied to it:

|Attribute name|Usage|
|-|-|
|DisplayName|Name of the property when it appears in the Properties window of the MonoGame Pipeline tool. If not specified, the internal property name, declared in the source code, is used. For this example, "Switch Coordinate System" would be displayed.|
|DefaultValue|A user interface (UI) hint specifying the possible default value of the property. This value is used only as a UI hint; it will not be set on the property, nor will it override the default value declared in the code.|
|Description|Descriptive text displayed when you select the property in the Properties window of the MonoGame Pipeline Tool.|

This completes the definition of the `SwitchCoordinateSystem` property.

In the next code example, the class definition is continued with an override of the [Process](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelProcessor) method:

```csharp
      //additional class code precedes...

      public override ModelContent Process(NodeContent input, ContentProcessorContext context)
      {
        if (switchCoordinateSystem)
        {
          Matrix switchMatrix = Matrix.Identity;
          switchMatrix.Forward = Vector3.Backward;
          MeshHelper.TransformScene(input, switchMatrix);
        }

        return base.Process(input, context);
      }
```

This code passes the `SwitchCoordinateSystem` property (declared earlier) value to [TransformScene](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.MeshHelper), which is a helper method that applies a transform to a scene hierarchy.

## See Also

- [Adding New Content Types](CP_Content_Advanced.md)  
- [Parameterized Content Processors](CP_StdParamProcs.md)  

---

© 2012 Microsoft Corporation. All rights reserved.

© 2023 The MonoGame Foundation.
