---
title: How to restrict Aspect Ratio on a Graphics Device
description: Demonstrates how to create a custom GraphicsDeviceManager that only selects graphics devices with widescreen aspect ratios in full-screen mode.
---

# Restricting Aspect Ratio on a Graphics Device

Demonstrates how to create a custom [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager) that only selects graphics devices with widescreen aspect ratios in full-screen mode.

## Restricting Graphics Devices

### To restrict graphics devices to widescreen aspect ratios in full-screen mode

1. Create a class that derives from [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager).

    ```csharp
    public class CustomGraphicsDeviceManager : GraphicsDeviceManager
    {
        public CustomGraphicsDeviceManager( Game game )
            : base( game )
        {
        }
    
    }
    ```

2. Add a **WideScreenOnly** property to the class.

    The property is used to turn on and off the widescreen-only behavior.

    ```csharp
    private bool isWideScreenOnly;
    public bool IsWideScreenOnly
    {
        get { return isWideScreenOnly; }
        set { isWideScreenOnly = value; }
    }
    ```

3. Determine the minimum desired aspect ratio.

    ```csharp
    static float WideScreenRatio = 1.6f; //1.77777779f;
    ```

4. Override the **RankDevices** method of [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager).

    Note the call to [base.RankDevices](xref:Microsoft.Xna.Framework.GraphicsDeviceManager). This call ensures that the new version of **RankDevices** has an already ranked list of available devices with which to work.

    ```csharp
    protected override void RankDevices( 
        List<GraphicsDeviceInformation> foundDevices )
    {
        base.RankDevices( foundDevices );
    }
    ```

5. Add a check to see if the **WideScreenOnly** property is **true**.

    ```csharp
    if (IsWideScreenOnly)
    {
        ...
    }
    ```

6. In the **if** block, loop through all found devices, and check whether the [PresentationParameters](xref:Microsoft.Xna.Framework.Graphics.PresentationParameters) indicate the device is full-screen.

7. If the device is full-screen, determine the aspect ratio of the device by dividing the **BackBufferWidth** by the **BackBufferHeight**.

8. If the aspect ratio is less than the desired aspect ratio, remove the device from the list of found devices.

    ```csharp
    for (int i = 0; i < foundDevices.Count; )
    {
        PresentationParameters pp = 
            foundDevices[i].PresentationParameters;
        if (pp.IsFullScreen == true)
        {
            float aspectRatio = (float)(pp.BackBufferWidth) / 
                (float)(pp.BackBufferHeight);
    
            // If the device does not have a widescreen aspect 
            // ratio, remove it.
            if (aspectRatio < WideScreenRatio) 
            { 
                foundDevices.RemoveAt( i ); 
            }
            else { i++; }
        }
        else i++;
    }
    ```

9. Replace the default [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager) with the derived [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager).
10. To test the new component, set the **WideScreenOnly** and **IsFullScreen** properties to **true**.

    ```csharp
            public Game1()
            {
                graphics = new CustomGraphicsDeviceManager(this);
                Content.RootDirectory = "Content";
    
                this.graphics.PreferMultiSampling = false;
    #if WINDOWS
                this.graphics.PreferredBackBufferWidth = 1280;
                this.graphics.PreferredBackBufferHeight = 720;
    #endif
    #if WINDOWS_PHONE
                this.graphics.PreferredBackBufferWidth = 400;
                this.graphics.PreferredBackBufferHeight = 600;
    #endif
    
                this.graphics.IsFullScreen = true;
                this.graphics.IsWideScreenOnly = true;
                graphics.ApplyChanges();
            }
    ```

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
