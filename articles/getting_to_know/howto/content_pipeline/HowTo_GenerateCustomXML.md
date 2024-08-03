---
title: How to create a custom XML File?
description: For complex game data, using a software tool to create and maintain these assets may be useful. Level tables, for example, might be easier to develop through a custom-level editor tool.
requireMSLicense: true
---

The [IntermediateSerializer](xref:Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate.IntermediateSerializer) class of the MonoGame Framework can be employed by custom tools running under Windows to directly serialize game data to an XML file.

An XML file generated in this way then can be included in the game project and imported by [XmlImporter Class](xref:Microsoft.Xna.Framework.Content.Pipeline.XmlImporter) as part of the Content Pipeline. Because [XmlImporter](xref:Microsoft.Xna.Framework.Content.Pipeline.XmlImporter) is actually a wrapper for [IntermediateSerializer](xref:Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate.IntermediateSerializer), it is certain that the XML file will be in the [correct format](../../whatis/Content_Pipeline/CP_XML_Elements.md) to be deserialized by the same facility.

The [IntermediateSerializer](xref:Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate.IntermediateSerializer) class is controlled through the `XmlWriter` class of the .NET Framework defined in `System.Xml`. The properties of the `XmlWriterSettings` class can be used to specify its output properties.

The serializer produces its output according to these rules:

- All public fields and properties are serialized; a separate XML element is used for each.
- Protected, private, or internal data is skipped.
- Get-only or set-only properties are skipped.
- Properties come before fields.
- If there is more than one field or property, these are serialized in the order they are declared.
- Nested types are serialized using nested XML elements.
- When the class derives from another, members of the base class are serialized before data from the derived type.

## XML Serialization Example

The following steps create a simple program that demonstrates how a program can use the [IntermediateSerializer](xref:Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate.IntermediateSerializer) method [IntermediateSerializer.Serialize Generic Method](xref:Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate.IntermediateSerializer) to serialize program data to an XML file.

### Step 1: Create a New Project

You will create a new project in Visual Studio.

1. On the `File` menu, click `New`, and then click `Project`.

2. In the `New Project` dialog box, ensure `Windows` is selected in the `Project types` pane, and then click `Console Application` in the `Templates` pane.

3. Open the new project, then in the `Solution Explorer`, right-click the `Dependencies` folder, and then click `Manage NuGet Packages..`.

4. Click on the `Browse` tab (if not selected) and search for the `MonoGame.Framework.Content.Pipeline` package, and then click `Install`.

    Accept any dialog prompts for Licenses or dependencies as they appear.

5. Additionally, install the `MonoGame.Framework.DesktopGL` NuGet package which is required by the `Content.Pipeline` package.

## Step 2: Add XML Serialization

1. In the `Solution Explorer`, double-click `Program.cs` to edit it.

2. Add `using` declarations for the System.Xml and Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate namespaces.

    ```csharp
        using System.Xml;
        using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
    ```

3. Define a class to serialize to XML with the properties you wish to store in your custom XML.

    ```csharp
        namespace XMLSerializerTest
        {
            class MyData
            {
                public int elf = 23;
                public string hello = "Hello World";
            }
        }
    ```

4. Within the function `Main` of the `Program` class, add the following code. This code employs [IntermediateSerializer.Serialize Generic Method](xref:Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate.IntermediateSerializer) to serialize the `MyData` class as an XML file.

```csharp
    class Program
    {
        static void Main(string[] args)
        {
            MyData ExampleData = new MyData();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create("example.xml", settings))
            {
                IntermediateSerializer.Serialize(writer, ExampleData, null);
            }
        }
    }
```

## Step 3: Generate XML

1. Press F5 to build and execute the program.
2. Examine the `example.xml` file in the project's `bin\\Debug\\netx.0` folder.

> [!NOTE]
> `netx.0` relates to the version of .NET your project is building under, `net6.0` for `.NET 6` and `net8.0` for `.NET 8`

You should have an output that looks like the following:

```xml
    <?xml version="1.0" encoding="utf-8"?>
    <XnaContent xmlns:XMLSerializerTest="XMLSerializerTest">
      <Asset Type="XMLSerializerTest:MyData">
        <elf>23</elf>
        <hello>Hello World</hello>
      </Asset>
    </XnaContent>
```

## See Also

### Concepts

- [Using an XML File to Specify Content](HowTo_UseCustomXML.md)  
- [Adding Content to a Game](HowTo_GameContent_Add.md)  

## Reference

- [IntermediateSerializer Class](xref:Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate.IntermediateSerializer)  
