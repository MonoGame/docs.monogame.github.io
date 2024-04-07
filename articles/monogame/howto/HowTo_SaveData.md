---
title: How to Save and Load data using MonoGame
description: Demonstrates reading and writing data in MonoGame projects.
---

# Writing Data

MonoGame is based on DotNet, which provides access to the System.IO namespace that delivers methods to interact with the file system

For detailed information about using Isolated Storage in games, see [Local Data Storage for Windows Phone](http://go.microsoft.com/fwlink/?LinkId=254759) in the Windows Phone documentation.

## Saving data using System.IO

Other saving

## Reading data using System.IO

Other saving

## To save game data with System.IO.IsolatedStorage

1. Add a **using System.IO.IsolatedStorage** statement at the beginning of the source file in which you'll be using the namespace.

    ```csharp
    using System.IO.IsolatedStorage;
    ```

2. Use [IsolatedStorageFile.GetUserStoreForApplication](http://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.isolatedstoragefile.getuserstoreforapplication.aspx) to get an [IsolatedStorageFile](http://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.isolatedstoragefile.aspx) object that can be used to create files and directories, or to read and write existing files.

    > When [IsolatedStorageFile.GetUserStoreForApplication](http://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.isolatedstoragefile.getuserstoreforapplication.aspx) is called within a MonoGame game for Windows (but not for Xbox 360 or Windows Phone), an InvalidOperationException will result. To avoid this exception, use the [GetUserStoreForDomain](http://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.isolatedstoragefile.getuserstorefordomain.aspx) method instead.

    ```csharp
    #if WINDOWS
        IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForDomain();
    #else
        IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForApplication();
    #endif
    ```

3. Use [IsolatedStorageFile.OpenFile](http://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.isolatedstoragefile.openfile.aspx) to open a file for writing, and use the returned [IsolatedStorageFileStream](http://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.isolatedstoragefilestream.aspx) to write data to the file.

    ```csharp
                protected override void OnExiting(object sender, System.EventArgs args)
                {
                    // Save the game state (in this case, the high score).
        #if WINDOWS
                    IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForDomain();
        #else
                    IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForApplication();
        #endif
    
                    // open isolated storage, and write the savefile.
                    IsolatedStorageFileStream fs = null;
                    using (fs = savegameStorage.CreateFile(SAVEFILENAME))
                    {
                        if (fs != null)
                        {
                            // just overwrite the existing info for this example.
                            byte[] bytes = System.BitConverter.GetBytes(highScore);
                            fs.Write(bytes, 0, bytes.Length);
                        }
                    }
        
                    base.OnExiting(sender, args);
                }
    ```

## Reading save game data with System.IO.IsolatedStorage

Reading saved data written in this way uses a very similar process. For example:

```csharp
        protected override void Initialize()
        {
            // open isolated storage, and load data from the savefile if it exists.
# if WINDOWS
            using (IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForDomain())
# else
            using (IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForApplication())
# endif
            {
                if (savegameStorage.FileExists(SAVEFILENAME))
                {
                    using (IsolatedStorageFileStream fs = savegameStorage.OpenFile(SAVEFILENAME, System.IO.FileMode.Open))
                    {
                        if (fs != null)
                        {
                            // Reload the saved high-score data.
                            byte[] saveBytes = new byte[4];
                            int count = fs.Read(saveBytes, 0, 4);
                            if (count > 0)
                            {
                                highScore = System.BitConverter.ToInt32(saveBytes, 0);
                            }
                        }
                    }
                }
            }
            base.Initialize();
        }
```

> [IsolatedStorageFile](http://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.isolatedstoragefile.aspx) provides many more methods than are shown here, including support for asynchronous reads and writes. For complete documentation, see [Local Data Storage for Windows Phone](http://go.microsoft.com/fwlink/?LinkId=254759) in the Windows Phone documentation, and the [IsolatedStorage](http://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.aspx) reference on MSDN.

## Serializing data

Text Data

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
