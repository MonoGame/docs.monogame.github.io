---
title: Debugging
description: During development, debugging a MonoGame project is essentially no different than debugging any other .NET project for the most cases, although graphics related debugging or troubleshooting can require the use of external tools.
---

When a game is under development, the developer usually needs some logging or tracing capabilities in order to troubbleshoot or debug the game. In addition to some basic logs output by the MonoGame framework itself, developers will likely need additional logging while they work on their games.

## Enabling Console Window During Debugging

When a MonoGame solution is created via one of the available templates, the project file or files that host the main game window are set up to use `WinExe` as the `OutputType`. This simply means the application has its own window that will display the game contents, with non interaction with the console or shell the underlying operating system provides.

The default platform project (`SolutionName.DesktopVK.csproj`, `SolutionName.WindowsDX12.csproj`, etc.) as it is created by the template would look like this:

[!code-xml[](./snippets/default_game_platform.csproj)]

Locate this line that sets the `OutputType` property in the project file:

[!code-xml[](./snippets/default_output_type.csproj)]

If we replace this line with a couple of conditional lines that set the `OutputType` property based on the build configuration, we can have a console window appear when debugging, while not having one when creating a release build.

[!code-xml[](./snippets/debug_output_type.csproj)]

In the example above, when the game is run in `Debug` mode, a console window will appear before the actual game window, with all the logging visible to the developer. When the game is run in `Release` mode, the game is built 

> [!NOTE]
> *Leaving the `OutputType` as `WinExe` for a release build is generally a bad idea. This will cause the game to open up a console window in addition to the actual game window, which is generally not a wanted behavior for most games from the perspective of the player. This is why, the default behavior for any `Release` build should be to set it to `WinExe`.*

## Adding Additional Logging

The developers can add logging/tracing capabilities to their games using a number of open source libraries that are widely available across the .NET ecosystem, or by building their custom logging implementations.
One easy way of having basic logging/tracing facilities in your game would be to rely on the standard methods in the `System.Diagnostics` namespace that comes with the .NET runtime as part of the base class library. The example below shows how this can be done:

[!code-csharp[](./snippets/debug_logging_simple.cs)]

In the example above, we're using this method to log information: `Debug.WriteLine()`<br/>
We could also use this method to have a similar result: `Trace.TraceInformation()`
But we should generally avoid this for the reasons we will explain in a bit.

But it is important to know the difference between the methods on the `Debug` and `Trace` classes:
* The methods on the `Debug` class will not be compiled into a Release build. This means, logs coming through these methods will not be output in a `Release` build, and all `Debug.Write()`, `Debug.WriteLine()` and similar calls will be stripped from the final executable, which makes them a good way of having logs when working on your game.
* The methods on the `Trace` class will be compiled into ***both** `Debug` and `Release` builds*, and that will allow you to have logs in the games you have shipped.

However, simply calling these methods will not be enough to actually display these log entries in the console window you enable in your MonoGame project through the changes in the project file. By default, the output of these methods will be directed to the output of the IDE you're using for development (e.g. Visual Studio), but they will not be directed to the console window.
In order to have them displayed in a console window, you will need to register a custom `TraceListener` in your game. The default `Program.cs` file for a MonoGame project doesn't include this, but it's very easy to add. This is how a default `Program.cs` file looks like:

[!code-csharp[](./snippets/default_program.cs)]

Using the example below, we will now register a `ConsoleTraceListener`in the `Program.cs` to direct the output of the logging methods to the console window:

[!code-csharp[](./snippets/program_with_consoletracelistener.cs)]

Once this is done, any logs you write with methods like `Debug.WriteLine()`, `Trace.TraceInformation()`, `Trace.TraceError()`, etc. will be visible in the console window, as long as you are running the game in the `Debug` mode.

> [!NOTE]
> *The code example above uses top-level statements which is the default for MonoGame project templates. If you are using an older template, you might need to add the code to the `Main` method of your `Program.cs` file instead.*

> [!WARNING]
> *Having logs in hot-paths like the `Update()` method will generate a significant overhead and will decrease your game's performance, in addition to causing pressure on the garbage collector, which in turn can end up causing stutter.
>
> Thus, make sure to add logging in the relevant methods that only get called when certain things happen in-game.
> And for the same reason, always prefer using `Debug.WriteLine()` over `Trace.TraceInformation()` unless you actually need that particular log in the release builds.
> 
> Because even if there is no console window to direct these logs to, `Trace.TraceInformation()` and similar methods will still incur a performance penalty in the release builds.*

## Advanced Logging

So far we have only considered a basic logging scenario where the logs will be visible in the console window. This is also why we rely exclusively on `Debug.WriteLine()`, since the console window is not visible in the Release builds.
But there can be scenarios when the developers might need more advanced logging capabilities for their games that are already shipped. For example, we may want to write a log file when the game crashes with an exception, which can be used by the players to report the issue to us.

For this purpose, we can register a `TextWriterTraceListener` or a custom other trace listener implementation that suits our needs. Here's how we can modify the `Program.cs` file to write logs to a file on the disk:

[!code-csharp[](./snippets/program_with_textwritertracelistener.cs)]

In this example, any exception that is not handled in the game itself through a `try/catch` block bubbles up to the top-level `Main` method where it's caught and logged to a file on the disk.
We can then ask players to send us these log files if they encounter crashes during gameplay.