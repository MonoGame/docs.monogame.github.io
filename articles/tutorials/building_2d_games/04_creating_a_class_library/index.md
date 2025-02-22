---
title: "04: Creating a Class Library"
description: "Learn how to create and structure a reusable MonoGame class library to organize game components and share code between projects."
---

One of the goals of this tutorial is to create reusable modules that you can use to jump start your next game project after this.  Instead of creating these modules directly in our game project, we can create a *class library* and add them to that.  Creating a class library offers several advantages, including:

1. **Reusability**: The classes can be easily reused in future game projects by simply adding a reference to the library.
2. **Organization**: Keeps game-specific code separate from reusable library code.
3. **Maintainability**: Changes to the library can benefit all games that use it.
4. **Testing**: The library code can be tested independently of any specific game.

A class library is a project type that compiles into a [Dynamic Link Library](https://learn.microsoft.com/en-us/windows/win32/dlls/dynamic-link-libraries) (DLL) instead of an executable.  It contains reusable code that can be referenced by other projects, making it perfect for sharing common functionality across multiple games.  MonoGame offers the *MonoGame Game Library* project template that can be used to create a class library.

In this chapter you will:

- Learn about class libraries and their benefits for game development.
- Create a MonoGame class library project using templates.
- Add library references to your game project.
- Structure your library for reusability.
- Set up the foundation for creating shared game components.

## Adding the Class Library

MonoGame offers the *MonoGame Game Library* project template to add a new class library project that is configured with the correct monoGame framework references.  Using this template saves time and ensures compatibility with MonoGame projects.  

To use the template to add the class library, perform the following:

### [Visual Studio Code](#tab/vscode)

To add the class library using the MonoGame Game Library project template in Visual Studio Code, perform the following:

1. In the *Solution Explorer* panel, right-click the *MonoGameSnake* solution.
2. Chose *New Project* from the context menu.
3. Enter "Monogame Game Library" and select it as the template to use.
4. Name the project "MonoGameLibrary".
5. When prompted for a location, use the default option, which will put the new project in a folder next to your game project.
6. Select "Create Project".

### [Visual Studio 2022](#tab/vs2022)

To add the class library using the MonoGame Game Library project template in Visual Studio 2022, perform the following:

1. Right-click the *MonoGameSnake* solution in the Solution Explorer panel.
2. Choose Add > New Project from the context menu.
3. Enter "MonoGame Game Library" in the search box, select that template, then click Next.
4. Name the project "MonoGameLibrary".
5. The location by default will put the new project in a folder next to your game project; you do not need to adjust this.
6. Click "Create".

### [dotnet CLI](#tab/dotnetcli)

To add the class library using the MonoGame Game Library project template with the dotnet CLI, perform the following:

1. Open a new Command Prompt or Terminal window in the same directory as the *MonoGameSnake.sln* solution file.
2. Enter the command `dotnet new mglib -n MonoGameLibrary` to create the project, placing it in a folder next to your game project.
3. Enter the command `dotnet sln MonoGameSnake.sln add ./MonoGameLibrary/MonoGameLibrary.csproj` to add the newly created class library project to the *MonoGameSnake.sln* solution file.

---

## Adding a Reference To The Class Library

Now that the game library project has been created, a reference to it needs to be added in our game project.  Without adding a reference, our game project will be unaware of anything we add to the class library.  To do this:

### [Visual Studio Code](#tab/vscode)

To add the game library project as a reference to the game project in Visual Studio Code:

1. In the Solution Explorer panel, right-click the *MonoGameSnake* project.
2. Choose "Add Project Reference" from the context menu.
3. Choose *MonoGameLibrary" from the available options.

> [!TIP]
> The Solution Explorer panel in VSCode is provided by the C# Dev Kit extension that was installed in [Chapter 02](../02_getting_started/index.md#install-the-c-dev-kit-extension).  If you do not see this panel, you can open it by
>
> 1. Opening the *Command Palette* (View > Command Palette).
> 2. Enter "Explorer: Focus on Solution Explorer View" and select the command.

### [Visual Studio 2022](#tab/vs2022)

To add the game library project as a reference to the game project in Visual Studio 2022:

1. In the Solution Explorer panel, right-click the *MonoGameSnake* project.
2. Select Add > Project Reference from the context menu.
3. Check the box for the *MonoGameLibrary* project.
4. Click Ok.

### [dotnet CLI](#tab/dotnetcli)

To add the game library project as a reference to the game project with the dotnet CLI:

1. Open a new Command Prompt or Terminal window in the same directory as the *MonoGameSnake.csproj* C# project file.
2. Enter the command `dotnet add ./MonoGameSnake.csproj reference ../MonoGameLibrary/MonoGameLibrary.csproj`.  This will add the *MonoGameLibrary* reference to the *MonoGameSnake* game project.

---

### Clean Up

When using the *MonoGame Game Library* project template, the generated project contains file similar to a standard MonoGame game project, including a *dotnet-tools.json* manifest file, a *Content.mgcb* file, and a *Game1.cs* file.  For the purposes of this tutorial, we will not need these.   To clean these up, locate the following in the *MonoGameLibrary* project directory and delete them:

1. The *.config/* directory.
2. The *Content/* directory
3. The *Game1.cs* file.

> [!TIP]
> These files are needed in more advanced scenarios such as creating a central code base for game logic that is referenced by other projects of which each target different platforms such as desktop, mobile, and console.  Creating a project structure of this type is out of scope for this tutorial.  
>
> If you would like more information on this, Simon Jackson has written the article [Going cross-platform with MonoGame](https://darkgenesis.zenithmoon.com/going-cross-platform-with-monogame.html) which covers this in more detail.

## Creating Our First Library Module

Let's validate our class library setup by creating a simple component that counts frames per second (FPS). This will demonstrate that we can successfully create classes in our library and use them in our game.

Create a new file called *FramesPerSecondCounter.cs* in the root of the *MonoGameLibrary* project and add the following code:

```cs
using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary;

/// <summary>
/// Tracks and calculates the number of frames rendered per second.
/// </summary>
public class FramesPerSecondCounter
{
    /// A static TimeSpan representing one second, used for FPS calculation intervals.
    private static readonly TimeSpan s_oneSecond = TimeSpan.FromSeconds(1);

    /// Tracks the number of frames rendered in the current second.
    private int _frameCounter;

    /// Tracks the elapsed time since the last FPS calculation.
    private TimeSpan _elapsedTime;

    /// <summary>
    /// Gets the current frames per second calculation.
    /// </summary>
    public float FramesPerSecond { get; private set; }

    /// <summary>
    /// Creates a new FramesPerSecondCounter.
    /// </summary>
    public FramesPerSecondCounter() { }

    /// <summary>
    /// Updates the FPS calculation based on elapsed game time.
    /// </summary>
    /// <param name="gameTime">A snapshot of the game's timing values.</param>
    public void Update(GameTime gameTime)
    {
        _elapsedTime += gameTime.ElapsedGameTime;

        if (_elapsedTime > s_oneSecond)
        {
            FramesPerSecond = _frameCounter;
            _frameCounter = 0;
            _elapsedTime -= s_oneSecond;
        }
    }

    /// <summary>
    /// Increments the frame counter. Should be called once per frame during the game's Draw method.
    /// </summary>
    public void UpdateCounter()
    {
        _frameCounter++;
    }
}
```

This class:

- Tracks how many frames are rendered each second.
- Updates the FPS calculation based on elapsed time.
- Provides the current FPS through a property.

Now let's use this counter in our game. Open *Game1.cs* and make the following changes:

1. Add the using directive for our library at the top:

    ```cs
    using MonoGameLibrary;
    ```

2. Add a field for the FPS counter:

    ```cs
    private FramesPerSecondCounter _fpsCounter;
    ```

3. In the constructor, create a new instance:

    ```cs
    _fpsCounter = new FramesPerSecondCounter();
    ```

4. In Update, add:

    ```cs
    _fpsCounter.Update(gameTime);
    ```

5. In Draw, just before `base.Draw()`, add:

    ```cs
    _fpsCounter.UpdateCounter();
    Window.Title = $"FPS: {_fpsCounter.FramesPerSecond}";
    ```

When you run the game now, you'll see the current FPS displayed in the window title bar. This confirms that:

- Our class library project is correctly set up.
- The game project can reference and use library classes.
- The basic structure for creating reusable components works.

| ![Figure 4-1: The game window showing the frames per second in the title bar of the window](./images/game_showing_fps.png) |
| :---: |
| **Figure 4-1: The game window showing the frames per second in the title bar of the window** |

> [!TIP]
> While this FPS counter is a simple example, it demonstrates the pattern we'll use throughout the tutorial: create reusable components in the library project, then reference and use them in games. This same approach will work for more complex components like sprite management, input handling, and collision detection.

> [!IMPORTANT]
> If you receive an error stating that the following:
>
> *The type or namespace name 'FramesPerSecondCounter' could not be found (are you missing a using directive or an assembly reference?)*
>
> This means either you forgot to add the `using MonoGameLibrary;` using directive to the top of the *Game1.cs* class file, or you did not add the project reference correctly.  Ensure that the project reference was added correctly by revisiting the [Add a Refernece to the Class Library](#adding-a-reference-to-the-class-library) section above and that you added the using directive.

## Conclusion

Let's review what you accomplished in this chapter:

- Learned about class libraries and their advantages for game development:
  - Code reusability across projects
  - Better organization and separation of concerns
  - Improved maintainability
  - Easier testing
- Created a MonoGame class library project
- Added the library as a reference to your game project
- Created your first reusable component and referenced and used it in the game project.

In the next chapter, we'll go over MonoGame's Game Component system and how we can use it with our class library.

## Test Your Knowledge

1. What are the main benefits of using a class library for game development?

   <details>
   <summary>Question 1 Answer</summary>

   > The main benefits are:
   > - **Reusability**: Code can be easily shared between different game projects
   > - **Organization**: Separates reusable code from game-specific code
   > - **Maintainability**: Changes to shared code benefit all games using the library
   > - **Testing**: Library code can be tested independently of specific games
   </details><br />

2. Why should you use the MonoGame Game Library template instead of a standard class library template?

   <details>
   <summary>Question 2 Answer</summary>

   > The MonoGame Game Library template automatically configures the correct MonoGame framework references and ensures compatibility with MonoGame projects, saving time and preventing potential setup issues.
   </details><br />

3. What happens if you don't add a reference to your class library in your game project?

    <details>
    <summary>Question 3 Answer</summary>

    > Without adding a reference, your game project will be unaware of any code in the class library. You won't be able to use any of the classes or components from the library in your game.
    </details><br />
