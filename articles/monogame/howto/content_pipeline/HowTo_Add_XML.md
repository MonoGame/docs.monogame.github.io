---
title: How to add a custom XML Content File to a Project?
description: Describes how to add custom game data as an XML file through the Content Pipeline.
requireMSLicense: true
---

## Adding XML files to game content

Custom game data that is expressed in an XML format can be easily integrated into your game through the MonoGame Content Pipeline.

This example demonstrates the procedure for integrating custom XML data into the content project of a simple game for the Windows platform.

### To define game data

Within the MonoGame solution, you create a new Windows Game Library project.

1. Right-click the solution node, point to `Add`, click `New Project`, and then select the `MonoGame Game Library` template.

    > [!TIP]
    > A `MonoGame Game Library` project is created instead of a Content Pipeline Extension Library project so that the class we will define can be used by both the [Content Importer](xref:Microsoft.Xna.Framework.Content.Pipeline.ContentImporter-1) that runs at build-time and the [Content Loader](xref:Microsoft.Xna.Framework.Content.ContentManager#Microsoft_Xna_Framework_Content_ContentManager_Load__1_System_String_) at game runtime.

2. In the `Name` box, type `MyDataTypes`, and then click `OK`.

3. In the `Solution Explorer`, delete the existing `Game1.cs` as it is not needed.

4. `right-click` and select `Add -> Add New Item` to add a new class, call it `PetData.cs`

5. Double click on `PetData.cs` and replace its contents with the following code to define the `PetData` class.

    ```csharp
    namespace MyDataTypes
    {
        public class PetData
        {
            public string Name;
            public string Species;
            public float Weight;
            public int Age;
        }
    }
    ```

6. Build the `MyDataTypes` project for debug or release to generate the library's dll file (note which profile you used).

### Add an XML file using the custom data to the game content

In this procedure, the `MyDataTypes` library is added as a reference in the content project.

1. In `Solution Explorer`, right-click the game content project, point to `Add Reference`, click the `Projects` tab, select `MyDataTypes`, and then click `OK`.

2. In the `Solution Explorer`, `right-click` the game content folder, point to `Add`, and then click `New Item`.

3. In the `Add New Item` dialog box, type `pets.xml` as the file name, and then click `OK`.

4. Replace the contents of the template file with the following XML code:

    ```xml
        <?xml version="1.0" encoding="utf-8" ?>
        <XnaContent>
          <Asset Type="MyDataTypes.PetData[]">
            <Item>
              <Name>Fifi</Name>
              <Species>Dog</Species>
              <Weight>11</Weight>
              <Age>6</Age>
            </Item>
            <Item>
              <Name>Bruno</Name>
              <Species>Dog</Species>
              <Weight>21</Weight>
              <Age>12</Age>
            </Item>
            <Item>
              <Name>Chloe</Name>
              <Species>Cat</Species>
              <Weight>6</Weight>
              <Age>3</Age>
            </Item>
            <Item>
              <Name>Pickles</Name>
              <Species>Hamster</Species>
              <Weight>0.4</Weight>
              <Age>1</Age>
            </Item>
          </Asset>
        </XnaContent>
    ```

    > [!TIP]
    > TO learn how to generate the custom MonoGame XML content from your own classes, refer to the [How to Generate custom XML](HowTo_GenerateCustomXML.md) guide.

5. Open the `Content.mgcb` content project by double-clicking on it.

    > [!TIP]
    > If you have any issues opening the MGCB content project, please refer to the [How to load content](HowTo_GameContent_Add.md) guide.

6. Click on `Add Existing` from the `Edit -> Add` options in the menu (or the `Add Existing` toolbar button) and select the `pets.xml` file.

7. Select the root `Content` element in the `Project` tree (left hand window) and then select `References` in the properties panel below.

8. Click on `Add` and browse to the `bin/debug/netx.0` folder of the `MyDataTypes` project.

> [!TIP]
> If the folder is empty, return to visual studio and build the `MyDataTypes` project, if it is not built, there is no dll.
> And make sure to choose either the `debug` or `release` folder depending on how the class library is built.

When you press `F6` to build the solution, it should build successfully, including the custom game content imported from the XML file.

> [!IMPORTANT]
> Adding the the class library of the data types project to the Content project is critical, else the Content Pipeline does not know how to serialize or deserialize the custom data you have defined.
> Although it is possible to simply serialize `value types` or `value type arrays` without this step.

To load the data at runtime, see the tutorial [Loading XML Content at Runtime](HowTo_Load_XML.md).

## See Also

- [Using an XML File to Specify Content](HowTo_UseCustomXML.md)  
- [Adding Content to a Game](HowTo_GameContent_Add.md)  
