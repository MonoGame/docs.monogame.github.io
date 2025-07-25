---
title: "Chapter 15: Audio Controller"
description: "Learn how to create a reusable audio controller class to manage sound effects and music, including volume control, muting/unmuting, and proper resource cleanup."
---

While playing sounds and music using the simplified sound API is straightforward, a game needs to handle various audio states and resource cleanup including:

- Track and manage sound effect instances that are created.
- Dispose of sound effect instances when they are finished.
- Handle volume control for songs and sound effects.
- Manage audio states (pause/resume, mute/unmute).

In this chapter you will:

- Learn how to create a central audio management system.
- Implement proper resource tracking and cleanup for sound effects.
- Build methods to control audio state (play/pause, mute/unmute).
- Add global volume control for different audio types.
- Integrate the audio controller with your game's core systems.
- Implement keyboard shortcuts for audio control.

By the end of this chapter, you will have an audio control system that can be easily reused in future game projects.

## The AudioController Class

To get started, in the *MonoGameLibrary* project:

1. Create a new folder named `Audio`.
2. Add a new class file named `AudioController.cs` to the `Audio` folder you just created.
3. Add the following code as the initial structure for the class

   [!code-csharp[](./snippets/audiocontroller.cs#declaration)]

   > [!NOTE]
   > The `AudioController` class will implement the `IDisposable` interface, This interface is part of .NET and provides a standardized implementation for an object to release resources. Implementing `IDisposable` allows other code to properly clean up the resources held by our audio controller when it is no longer needed. For more information on `IDisposable`, you can read the [Implement a Dispose Method](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose) article on Microsoft Learn.

### AudioController Properties and Fields

The `AudioController` will need to track sound effect instances created for cleanup and track the state and volume levels of songs and sound effects when toggling between mute states.

Add the following fields and properties:

[!code-csharp[](./snippets/audiocontroller.cs#properties)]

### AudioController Constructor

The constructor just initializes the collection used to track the sound effect instances.

Add the following constructor and finalizer:

[!code-csharp[](./snippets/audiocontroller.cs#ctors)]

> [!NOTE]
> The `AudioController` class implements a [finalizer](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/finalizers) method `~AudioManager()`. This method is called when an instance of the class is collected by the garbage collector and is here as part of the `IDisposable` implementation.

### AudioController Methods

The `AudioController` needs methods to:

- Update it to check for resources to clean up.
- Playing sound effects and songs
- State management (play/pause, mute/unmute)
- Volume control
- Implement the `IDisposable` interface.

So lets add them below.

#### AudioController Update

The `Update` method will check for existing sound effect instances that have expired and properly dispose of them. Add the following method:

[!code-csharp[](./snippets/audiocontroller.cs#update)]

#### AudioController Playback

While the MonoGame simplified audio API allows sound effects to be played in a fire and forget manner, doing it this way does not work if you need to pause them because the game paused. Instead, we can add playback methods through the `AudioController` that can track the sound effect instances and pause them if needed, as well as checking the media player state before playing a song.

Add the following methods:

[!code-csharp[](./snippets/audiocontroller.cs#playback)]

#### AudioController State Control

The `AudioController` provides methods to control the state of audio playback including pausing and resuming audio as well as muting and unmuting.

Add the following methods:

[!code-csharp[](./snippets/audiocontroller.cs#state)]

#### AudioController IDisposable Implementation

Finally, the `AudioController` is required to implement the `IDisposable` interface, to complete this add the following methods:

[!code-csharp[](./snippets/audiocontroller.cs#idisposable)]

Games often use limited system resources like audio channels, when we are done with these resources we need to clean them up properly. In .NET, the standard way to handle resource cleanup is through the `IDisposable` interface.

Think of `IDisposable` like a cleanup checklist that runs when you are finished with something:

1. The interface provides a `Dispose` method that contains all cleanup logic.
2. When called, `Dispose` releases any resources the class was using.
3. Even if you forget to call `Dispose`, the finalizer acts as a backup cleanup mechanism.

For our `AudioController`, implementing `IDisposable` means we can ensure all sound effect instances are properly stopped and disposed when our game ends, preventing resource leaks.

> [!NOTE]
> Fore more information on `IDisposable` and the `Dispose` method, check out the [Implementing a Dispose Method](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose) article on Microsoft Learn.

## Implementing the AudioController Class

Now that we have the audio controller class complete, we can update the game to use it. We will do this in two steps:

1. First, update the `Core` class to add the `AudioController` globally.
1. Update the `Game1` class to use the global audio controller from `Core`.

### Updating the Core Class

The `Core` class serves as our base game class, so we will update it first to add and expose the `AudioController` globally. Open the `Core.cs` file in the *MonoGameLibrary* project and update it to the following:

[!code-csharp[](./snippets/core.cs?highlight=6,50-53,115–116,119–125,132–133)]

The key changes made here are:

1. Added the `using MonoGameLibrary.Audio;` directive to access the `AudioController` class.
2. Added a static `Audio` property to provide global access to the audio controller.
3. Created the new audio controller instance in the `Initialize` method.
4. Added an override for the `UnloadContent` method where we dispose of the audio controller.
5. The audio controller is updated in the `Update` method.

### Updating the Game1 Class

Next, update the `Game1` class to use the audio controller for audio playback. Open `Game1.cs` and make the following updates:

[!code-csharp[](./snippets/game1.cs?highlight=45-46,77-78,104-105,194–195,213–214,267–285)]

> [!NOTE]
> Note there were a lot of replacements in the `LoadContent` method, switching from loading and initializing the background Song and replacing it with a call to the new `AudioController` to do all the work managing the Song reference. Much cleaner.

The key changes made here are:

1. The `_themeSong` field is added to store a reference to the background song to play.
2. In [**LoadContent**](xref:Microsoft.Xna.Framework.Game.LoadContent), the background theme song is loaded using the content manager.
3. In [**Initialize**](xref:Microsoft.Xna.Framework.Game.Initialize), the audio manager is used to play the background theme song.
4. In [**Update**](<xref:Microsoft.Xna.Framework.Game.Update(Microsoft.Xna.Framework.GameTime)>) the audio manager is used to play the bounce and collect sound effects.
5. In `CheckKeyboardInput` the following checks were added
   1. If the M key on the keyboard is pressed, it will toggle mute for all audio.
   2. If the + key is pressed, the song and sound effect volumes are increased by `0.1f`.
   3. If the - key is pressed, the song and sound effect volumes are decreased by `0.1f`.

Running the game now will produce the same result as the previous chapter, only now the lifetime of sound effects and the state management of audio is done through the new audio controller. You can also mute and unumte the audio with the M key and increase and decrease the volume using the + and - keys.

| ![Figure 15-1: Gameplay with audio.](./videos/gameplay.webm) |
| :----------------------------------------------------------: |
|            **Figure 15-1: Gameplay with audio.**             |

> [!NOTE]
> You may note that while we added keybindings to change the audio settings, we did not add any bindings for the GamePad. This is simply becuase this is not normally how you would adjust these values on a console, on consoles you would have a settings/options screen to update them.
>
> Later in [Chapter 20: Implementing UI with GUM](../20_implementing_ui_with_gum/index.md) we will add an Options screen to adjust all the audio values for the game.

## Conclusion

In this chapter, you accomplished the following:

- Created a reusable `AudioController` class to centralize audio management.
- Learned about proper resource management for audio using the `IDisposable` pattern.
- Implemented tracking and cleanup of sound effect instances.
- Added global volume control for both sound effects and music.
- Created methods to toggle audio states (play/pause, mute/unmute).
- Updated the `Core` class to provide global access to the audio controller.
- Added keyboard controls to adjust volume and toggle mute state.

The `AudioController` class you created is a significant improvement over directly using MonoGame's audio APIs. It handles common audio management tasks that would otherwise need to be implemented repeatedly in different parts of your game. By centralizing these functions, you make your code more maintainable and provide a consistent audio experience across your game.

In the next chapter, we will start exploring fonts and adding text to the game.
