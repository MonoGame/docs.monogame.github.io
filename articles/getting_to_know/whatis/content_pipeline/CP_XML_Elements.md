---
title: What are the XML Elements for XMLImporter?
description: The base elements that are recognized by XmlImporter Class.
---

# XML Elements for XMLImporter

## XML Elements

The following base elements are recognized by [XmlImporter Class](xref:Microsoft.Xna.Framework.Content.Pipeline.XmlImporter):

|Element|Parent|Children|Description|
|-|-|-|-|
|`<XnaContent>`|—|`<Asset>`|Top-level tag for XNA Content.|
|`<Asset>`|`<XnaContent>`|`<Item>`|Marks the asset. The _Type_ attribute specifies the corresponding namespace and class of the matching data type.|
|`<Item>`|`<Asset>`|—|When the asset contains multiple objects (as in an array), marks a single object within the group. The child elements correspond to the properties of the data type's class definition.|

## Examples

## Example 1: Single Object

This example demonstrates an XML file that defines an asset that consists of a single item (not an array).

Assume that the XML file is to define a single object of data for the class that is defined as:

```csharp
namespace XMLTest
{
    class MyTest
    {
        public int elf;
        public string hello;
    }
} 
```

The XML file that specifies the data that the Content Loader will read into the object would appear as:

```xml
<?xml version="1.0" encoding="utf-8"?>
<XnaContent>
  <Asset Type="XMLTest.MyTest">
    <elf>23</elf>
    <hello>Hello World</hello>
  </Asset>
</XnaContent>
```

## Example 2: Multiple Objects

This example demonstrates an XML file that defines an asset that is an array of objects.

Assume that the XML file is to define an array of data for the class that is defined as:

```csharp
namespace MyDataTypes
{
    public class CatData
    {
        public string Name;
        public float Weight;
        public int Lives;
    }
}
```

The XML file that specifies the data that the Content Loader will read into the object array would appear as:

```xml
<?xml version="1.0" encoding="utf-8"?>
<XnaContent>
  <Asset Type="MyDataTypes.CatData[]">
    <Item>
      <Name>Rhys</Name>
      <Weight>17</Weight>
      <Lives>9</Lives>
    </Item>
    <Item>
      <Name>Boo</Name>
      <Weight>11</Weight>
      <Lives>5</Lives>
    </Item>
  </Asset>
</XnaContent>
```

## See Also

- [Using an XML File to Specify Content](../../howto/Content_Pipeline/HowTo_UseCustomXML.md)  
- [Adding Content to a Game](../../howto/Content_Pipeline/HowTo_GameContent_Add.md)  

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
