---
title: Saving Game Data
description: Save and load player progress with the StorageDevice and StorageContainer classes, to protect save data from crashes and power loss cross-platform.
---

Games normally require the ability to save a player's progress, high score or current game settings. As of `3.8.6`, MonoGame has restored and modernised the `StorageContainer` capability from XNA, allowing you to save this data safely on native platforms.

This guide shows you how to open a save container, save and load data, and handle the errors that can occur.

> [!IMPORTANT]
> Storage Container is currently in preview, and the API can change before the final release.
>
> The storage classes are available on the *DesktopVK*, *WindowsDX12*, *DesktopGL4* and *WebGL* native platforms only. They are not available on *DesktopGL*, *WindowsDX*, *Android* or *iOS*, until those platforms are updated with native implementations.

## How storage works

The storage classes are in the `Microsoft.Xna.Framework.Storage` namespace:

> [!IMPORTANT]
> The namespace and classes are **ONLY** visible to the `Native` platforms (DesktopGL4, VK, DX12, Web) for other platforms the code will not be recognised or compiled, which becomes an issue if you share code between the `Native` and `Non-Native` targets.

| Class | What it is |
| --- | --- |
| `[StorageDevice](xref:Microsoft.Xna.Framework.Storage.StorageDevice)` | The save location for your game. You create one device for each game title. |
| `[StorageContainer](xref:Microsoft.Xna.Framework.Storage.StorageContainer)` | A named group of files inside a device. All the files in a container save together as one unit. |

Inside a container, you read and write each file with a normal .NET `Stream`. Thus, you can use a `BinaryWriter`, `StreamWriter` or JSON serializer, as with any other file.

Then the data is written to disk through a `StorageDevice` using the following structure (with a different root depending on your platform):

```text
Documents/SavedGames/        <- created by the first device
└── <titleName>/               <- one StorageDevice (its root)
   └── <containerName>/         <- a StorageContainer
    └──  save.dat               <- files you open through the container
    └──  levels/progress.dat
   └── ~<containerName>/        <- backup during a commit
   └── ~~/                      <- scratch folder for commits, shared by all containers
```

The storage classes protect your save data in three ways:

- **Changes stay in memory until you call `Commit`.** - If you dispose a container without a commit, the changes are lost and the saved files do not change.
- **A commit saves all the changes, or none of them.** - If the game stops during a commit, for example in a crash or a power cut, the next `OpenContainer` call restores the last good save.
- **Containers can only be opened one at a time on a device.** - Containers are locked when open, this stops two parts of your game from trying to change the save data at the same time. Always dispose a container when you are done with it.

> [!NOTE]
> You can have multiple StorageContainers in your project for different types of data, but only **ONE** `StorageDevice` as it is title dependent.

## Where the files are saved

On desktop platforms, the device makes a `SavedGames` folder with one folder for each game title. Each container is a folder inside the title folder.

| Operating system | Location |
| --- | --- |
| Windows | `Documents\SavedGames\<title>\<container>\` |
| macOS and Linux | `$XDG_DATA_HOME/SavedGames/<title>/<container>/`, or `~/.local/share/SavedGames/<title>/<container>/` if `XDG_DATA_HOME` is not set |

Other platforms allow for saving data in a different place, some platforms also allow you to store/synchronize the data to the cloud.

> [!NOTE]
> During a commit, you can see folders that start with `~` next to your container folder. The device uses these folders to make the commit safe. It removes them when the commit is complete.

## Open a save container

The following example saves two values from the `Game1` class: a current level and high score value.

1. At the top of `Game1.cs`, add the following usings:

    ```cs
    using System.Diagnostics;
    using System.IO;
    using Microsoft.Xna.Framework.Storage;
    ```

2. Add a field for the device, two constants for the names of the container and the file, and the two values to save:

    ```cs
    private const string SaveContainerName = "SaveData";
    private const string ProgressFileName = "progress";

    private StorageDevice _storage;
    private int _currentLevel;
    private int _highScore;
    ```

3. Create a `GetSaveContainer` method that creates the device and opens the container, if none exists it creates one:

    > [!NOTE]
    > If the StorageContainer could not be opened for any reason, this will return `null`, so be prepared to handle that.

    ```cs
    // Opens the container for save data. Returns null if it cannot open.
    private StorageContainer GetSaveContainer()
    {
        if (_storage == null)
        {
            _storage = new StorageDevice("MyGame");
        }

        // Ensure we have at least 1 MB of storage space available.
        return _storage.OpenContainer(SaveContainerName, 1 * 1024 * 1024);
    }
    ```

The `StorageDevice` constructor takes the name of your game which is used by the StorageDevice for the title's folder, so make sure to use the same name in each version of your game. (or as a mechanism to version your saves and preserve old versions).

`OpenContainer` opens the container if it exists, or creates it if it does not. You also have to specify a value for the space that the save data needs, in bytes, **it must be greater than 0**. When the device creates a new container, it checks that there is sufficient space free as a safety precaution against filling the disk. If the device cannot open the container, or reserve sufficient space, `OpenContainer` returns `null`.

> [!TIP]
> Both `StorageDevice` and `StorageContainer` take an optional `PlayerIndex`, this is intended for platforms that keep save data for each player. Pass `null` if the data is for all players.
>
> Desktop platforms ignore this value.

## Save data to the container

With the container open on a storage device, you can then call to save the data to disk.  Add the following method which will write the two values:

```cs
private void SaveGame()
{
    try
    {
        using (StorageContainer container = GetSaveContainer())
        {
            if (container == null)
            {
                ShowSaveError("Could not open the save container.");
                return;
            }

            using (Stream stream = container.OpenFile(ProgressFileName, FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(_currentLevel);
                    writer.Write(_highScore);
                }
            }

            // Write the changes to storage.
            container.Commit();
        }
    }
    catch (Exception ex)
    {
       ShowSaveError($"Could not save the game: {ex.Message}");
    }
}

private void ShowSaveError(string errorText)
{
    Debug.WriteLine(errorText);
}
```

> [!IMPORTANT]
> Saving should be called **On Demand** and **NOT** as part of any loop such as `Update` or `Draw`, this is critical!  The user should be choosing to save, and load is a one time activity.

This safely manages the saving session, wrapped in a `using` to ensure the Save StorageContainer is released once save has completed, `StorageContainer` implements `IDisposable` to ensure it is memory safe.

Of note, in the options above:

- `FileMode.Create` makes a new file, or replaces the file if it exists. Thus, each save replaces the last one. When the `BinaryWriter` closes, it also closes the stream.

- `Commit` writes the changes to storage. Call it after you close the writer, so that all the data is in the file. The `using` statement then disposes the container, so that the device can open a container again.

> [!WARNING]
> Do not use `FileMode.CreateNew` to save. It throws an `IOException` if the file already exists, else the first save will work but each save after that fails.

In a real game, show the player a message if the save fails. A failed save that the player does not know about can lose their progress.

### When to save

Each save writes to the disk, so do not save during each frame (such as the Update loop). Save on demand when something important changes, for example:

- When the player completes a level or reaches a checkpoint.
- When the player gets a new high score.
- When the player changes a setting.

> [!WARNING]
> Do not only save when the game closes, because if the game stops unexpectedly, the player will lose all progress since the last save.

## Load data

Loading data is simply the opposite of saving, open the required container, retrieve the saved data and update the game state.

1. Add this method to load the two values:

    ```cs
    private void LoadGame()
    {
        try
        {
            using (StorageContainer container = GetSaveContainer())
            {
                if (container != null && container.FileExists(ProgressFileName))
                {
                    using (Stream stream = container.OpenFile(ProgressFileName, FileMode.Create))
                    {
                        using (BinaryReader reader = new BinaryReader(stream))
                        {
                            _currentLevel = reader.ReadInt32();
                            _highScore = reader.ReadInt32();
                        }
                    }

                    // The save data loaded.
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ShowSaveError($"Could not load the game: {ex.Message}");
        }

        // There is no save data, or it could not be read. Use the defaults.
        LoadDefaultSaveState();
    }

    private void LoadDefaultSaveState()
    {
        _currentLevel = 1;
        _highScore = 0;
    }
    ```

2. Then in `Initialize`, call `LoadGame` **before** `base.Initialize()`:

    ```cs
    protected override void Initialize()
    {
        LoadGame();

        base.Initialize();
    }
    ```

- `FileExists` tells you if there is save data, which when the game starts for the first time, there will be no file, so you load the default values.

Especially for Binary Serialization, read the values in the same order that you wrote them (but it is good practice to do this anyway). The `Load` method only returns after it reads all the values. If the file is damaged, the reader throws an exception and again the game uses the default values.

> [!TIP]
> If you change the format of your save data in an update to your game, write a version number at the start of the file. When you load, read the version first, then read the data in the format for that version. And PLEASE, include a migration method to convert an old save to the new, if you can.

## Close the storage device

On closing your game, make sure you release the memory reserved for the `StorageDevice`, as it holds resources until it is disposed.

1. Dispose the `StorageDevice` when Content is Unloaded before the game closes (you can do this on Quit, but it is safer to include it during the general wind down/unloading of the game normally):

```cs
protected override void UnloadContent()
{
    _storage?.Dispose();
    _storage = null;

    base.UnloadContent();
}
```

When you dispose the device, it also closes any container that is still open for the device.

> [!WARNING]
> However, it does not commit the changes in that container, so anything pending will be lost.

## Use folders and more files

A container can hold many files, it also supports organising files in folders within the container. The only rule is that you must create a folder with `CreateDirectory` before you put a file in it.

The following example saves two `float` volume settings to a file (`audio`) in a `settings` folder within the container:

```cs
using (StorageContainer container = GetSaveContainer())
{
    if (container != null)
    {
        container.CreateDirectory("settings");

        Stream stream = container.OpenFile("settings/audio", FileMode.Create);
        using (BinaryWriter writer = new BinaryWriter(stream))
        {
            writer.Write(_musicVolume);
            writer.Write(_effectsVolume);
        }

        container.Commit();
    }
}
```

`CreateDirectory` does nothing if the folder exists. Thus, you can call it each time you save.

To find files, use `GetFileNames`, which also supports using a search pattern such as `*` for any characters and `?` for one character:

```cs
// For example, "slots/slot1", "slots/slot2" and "slots/slot3".
string[] saveSlots = container.GetFileNames("slots/*");
```

> [!NOTE]
> Put files that always change together in the same container as commit protects all the files in one container.

## Delete save data

While you can do simple FileIO operations to delete content, it is better handle delete operations through the Storage API. To delete a file, call `DeleteFile`, then `Commit`:

```cs
using (StorageContainer container = GetSaveContainer())
{
    if (container != null)
    {
        container.DeleteFile(ProgressFileName);
        container.Commit();
    }
}
```

To delete a container and all its files, you can alteratively use `DeleteContainer` on the device. 

```cs
_storage.DeleteContainer(SaveContainerName);
```

> [!WARNING]
> This deletes the container immediately and does not need a commit. Make sure that no container from the device is open!

## Rules for names and paths

The following guidelines must be adhered to when working with Storage Devices and Containers:

- A container name must contain only ASCII characters. It cannot contain `/` or `\`, start with `~`, or be `.` or `..`.
- File and folder names inside a container are **not** case sensitive. The container forces them to lower case. Thus, `GetFileNames` returns `progress`, not `Progress`.
- Use `/` to separate folders in a path. The container automatically changes `\` to `/`, which makes this simpler when working across different operating systems.
- To put a file in a folder, first create the folder with `CreateDirectory`. If the folder does not exist, `OpenFile` and `CreateFile` will throw a `DirectoryNotFoundException`.
- `CreateDirectory`, `DeleteDirectory` and `DeleteFile` also change only the memory copy. Make sure to call `Commit` to save them.

## Common errors

| Exception | Cause |
| --- | --- |
| `ArgumentNullException` | The container name is empty, or it breaks a rule in [Rules for names and paths](#rules-for-names-and-paths). |
| `ArgumentOutOfRangeException` | The required space for `OpenContainer` is 0 or less. |
| `NotSupportedException` | A container from the same device is still open. Dispose it before you open a container or delete one. |
| `DirectoryNotFoundException` | The file is in a folder that you did not create with `CreateDirectory`. |
| `FileNotFoundException` | You used `FileMode.Open` for a file that does not exist. Call `FileExists` first. |
| `IOException` | You used `FileMode.CreateNew` for a file that exists. Use `FileMode.Create` to replace a file. |

## The storage API

The following is a simplified version of the main API documentation located at [https://docs.monogame.net/api/index.html](https://docs.monogame.net/api/index.html):

### [StorageDevice](xref:Microsoft.Xna.Framework.Storage.StorageDevice)

| Member | Description |
| --- | --- |
| `StorageDevice(string titleName, PlayerIndex? player = PlayerIndex.One)` | Creates a device for the save data of a game. Pass `null` for the player if the data is for all players. |
| `OpenContainer(string containerName, long requiredFreeBytes)` | Opens a container, or creates it. Returns `null` if it cannot open the container. |
| `DeleteContainer(string containerName)` | Deletes a container and all its files immediately. No commit is necessary. |
| `FreeSpace` | The free space on the device, in bytes. |
| `TotalSpace` | The total space on the device, in bytes. |
| `IsDisposed` | `true` after you dispose the device. |
| `Dispose()` | Closes the device and any container that is still open. |

### [StorageContainer](xref:Microsoft.Xna.Framework.Storage.StorageContainer)

| Member | Description |
| --- | --- |
| `OpenFile(string fileName, FileMode fileMode)` | Opens a file with a `FileMode`, and returns a `Stream`. |
| `CreateFile(string fileName, bool truncate = true)` | Creates a file and returns a `Stream`. If `truncate` is `false` and the file exists, the stream adds to the end of the file. |
| `FileExists(string fileName)` | `true` if the file exists. |
| `DeleteFile(string fileName)` | Deletes a file. |
| `GetFileNames()` | Returns the paths of all the files in the container. |
| `GetFileNames(string searchPattern)` | Returns the paths of the files that match a pattern. Use `*` for any characters and `?` for one character. |
| `CreateDirectory(string directoryName)` | Creates a folder. |
| `DirectoryExists(string directoryName)` | `true` if the folder exists. |
| `DeleteDirectory(string directoryName)` | Deletes a folder and all its contents. |
| `GetDirectoryNames()` | Returns the paths of all the folders in the container. |
| `GetDirectoryNames(string searchPattern)` | Returns the paths of the folders that match a pattern. |
| `Commit()` | Writes all the changes to storage, as one safe operation. |
| `ContainerName` | The name of the container. |
| `StorageDevice` | The device that opened the container. |
| `IsDisposed` | `true` after you dispose the container. |
| `Dispose()` | Closes the container. It does not save changes. |

## See also

- [Logging](logging.md): write messages such as save errors to a log while you develop your game.
- [Preparing for consoles](preparing_for_consoles.md): what changes when your game moves to a console.
