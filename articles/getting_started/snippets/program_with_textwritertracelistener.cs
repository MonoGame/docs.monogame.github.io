using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FooBar.Game;

namespace FooBar;
public class Program
{
    public static void Main(string[] args)
    {
        Trace.Listeners.Add(new ConsoleTraceListener());
        Trace.Listeners.Add(new TextWriterTraceListener($"FooBar_CrashLog_{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}.log")
        {
            Name = "CrashLogger",
            Filter = new EventTypeFilter(SourceLevels.Critical | SourceLevels.Error),
        });
        Trace.AutoFlush = true;

        // Catch exceptions on the main thread.
        AppDomain.CurrentDomain.UnhandledException += (sender, exArgs) =>
        {
            var ex = exArgs.ExceptionObject as Exception;
            LogFatalException($"Unhandled Exception from sender: {sender}\nException: {ex?.Message}\n{ex?.StackTrace}");
        };

        // Catch exceptions from background tasks/threads.
        TaskScheduler.UnobservedTaskException += (sender, exArgs) =>
        {
            LogFatalException($"Unobserved Task Exception from sender: {sender}\nException: {exArgs.Exception.Message}\n{exArgs.Exception.StackTrace}");
        };
        
        using var game = new GameClass();
        game.Run();
    }

    private static void LogFatalException(string errorMessage)
    {
        Trace.TraceError(errorMessage);
    }
}
