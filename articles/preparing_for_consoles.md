---
title: Preparing for consoles
description: How to get your game as ready as possible to run on consoles.
---

# Preparing for consoles

If you would like to port your game on consoles, there are some best practices to follow if you want to avoid running into issues while porting.

MonoGame for gaming consoles uses a dedicated .NET runtime that is not maintained by Microsoft. Therefore, a 100% accuracy and reliability is not guaranteed. Moreover, the console runtime makes use of ahead-of-time native compilation (AOT), which means that some .NET features won't, and can't work on consoles.

This article explains the most common pitfalls and suggested guidelines to optimize your chances of having a smoother porting experience.

*Gaming consoles are restricted to registered developers and are not publicly available nor publicly documented. To get access to those platforms, please contact your console account manager(s). MonoGame documentation for closed platforms is available in their respective repositories.*

## No use of runtime reflection

The main show stopper when it comes to porting a .NET game to consoles, is the use of runtime [reflection](https://learn.microsoft.com/en-us/dotnet/fundamentals/reflection/reflection).

Reflection is, at large, unsupported in an AOT context. Which means that anywhere you or a third party library uses reflection, your game will crash.

In order to make sure that your game abides to that rule, you can try to publish an AOT'd version of it for desktop computers and verify how it fares.

To publish with AOT, add `<PublishAot>true</PublishAot>` to your `.csproj` and run `dotnet publish`. This will nativily compile your game in a fashion similar to consoles. The output executable will be in your output folder, within a sub-folder nammed `publish`.

From there, you can try running this executable. If it doesn't start or crash later on, you likely are using reflection or unsupported features.

Native executables can debugged by starting an empty Visual Studio, opening that exe, and hitting "Start debugging". It should show you on which C# lines it crashed.

It is important to note that you should test every aspects of your game, and not just if it starts.

Another way to make sure that everything is safe is to enable `<EnableTrimAnalyzer>true</EnableTrimAnalyzer>` in your `.csproj`, and then rebuild (not just build) your game and check the build output for AOT warnings. Those warnings will tell you which parts of your code might trigger crashes or unexecpted results when running on AOT compilation. You should seek to resolve all of them.

## No runtime compilation / IL emit

Generating code at runtime is a scenario that is not supported in AOT contexts, and that is also forbidden by console manufacturers.

Likewise to reflection, trying to make a `PublishAot` build if a good way to verify that your game is compliant because any use of IL emit will crash.

## No use of dynamic assembly loading

Loading assemblies at runtime with `Assembly.Load()` is not supported.

## Third party libraries

Many third party libraries are heavily using reflection or IL emit. For instance, this is a common practice for JSON or XML parsers.

It is advised to choose very carefully the libraries that you are using when porting to consoles. If you don't select them with this in mind, you might run into a situation in which you'll have to rewrite entire chunks of data handling.

The best way to make sure if they will work, is to search if they are "AOT-compatible", or try to compile with `<EnableTrimAnalyzer>true</EnableTrimAnalyzer>` and check if there are any warnings related to those libraries.

## Native libraries

If you are using native libraries, make sure that they propose builds for consoles, or make sure that you can compile and run them on consoles yourself.

Even though a library might be open-source, it is not likely to just compile and run on consoles.

It is suggested to only use native libraries that have been proven to run on consoles.

## LINQ

While LINQ mostly works on consoles, it should be noted that due to the AOT compilation, LINQ queries can't be optimized and their performances are very slow.

Moreover, LINQ is very garbage-prone and if your game has stuttering issues you might want to reduce your usage of LINQ.

## Avoiding garbage generation

Even though your game has good performances on PC and doesn't show stutters, you might want to be cautious about how your game handles memory.

The garbage collector is slower on consoles and if your game generates a lot of garbage memory, there will be visible stutters.

To verify that your game is not too garbage-prone, you can run Visual Studio's Perfomance Profiler (`Debug/Performance Profiler...`) and check the ".NET Object Allocation Tracking" tool.

From there, you can check which parts of your code generate garbage and you can pinpoint where to optimize.

In order to avoid garbage, here are some best practices:

- Only use strings as const. Don't use string dynamically (e.g. string concatenation, patterns...), this is the most common source of garbage
- Avoid allocating anything with the `new` keyword during your game loop (e.g. pre-allocate everything ahead of using them during gameplay)
- Pool your dynamic objects (e.g. don't destroy your projectiles or particles but place them into another "unused" list and reuse them instead of creating new instances when needed)
- Avoid using LINQ
- If you are using collections, initialize them with a large enough capacity to avoid their internal data structure to be silently recreated
- Mind your foreach loops, depending on the data your looping on, the loop might create garbage when duplicating an item

## Don't rely on system calls

If your game calls directly to system functions, like kernel, win32 or unix commands, you might want to get rid of them.

## Consider I/O to be asynchronous

Saving player data/settings, or unlocking achievements are operations that should be considered asynchronous.

Most, if not all consoles, consider system accesses to be asynchronous. Even though it is not on PC, you should prepare your game to handle asynchronous operation (e.g. consider saving game data in a thread which will not block the game).

If you consider all your I/O and system operations as asynchronous, you will likely be spared some headache.

