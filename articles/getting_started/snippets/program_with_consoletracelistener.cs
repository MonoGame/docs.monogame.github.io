using System.Diagnostics;
using FooBar.Game;

Trace.Listeners.Add(new ConsoleTraceListener());

using var game = new GameClass();
game.Run();
