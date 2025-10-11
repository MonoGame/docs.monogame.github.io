[Conditional("DEBUG")]
public static void StartContentWatcherTask()
{
    var args = Environment.GetCommandLineArgs();
    foreach (var arg in args)
    {
        // if the application was started with the --no-reload option, then do not start the watcher.
        if (arg == "--no-reload") return;
    }

    // identify the project directory
    var projectFile = Assembly.GetEntryAssembly().GetName().Name + ".csproj";
    var current = Directory.GetCurrentDirectory();
    string projectDirectory = null;

    while (current != null && projectDirectory == null)
    {
        if (File.Exists(Path.Combine(current, projectFile)))
        {
            // the valid project csproj exists in the directory
            projectDirectory = current;
        }
        else
        {
            // try looking in the parent directory.
            //  When there is no parent directory, the variable becomes 'null'
            current = Path.GetDirectoryName(current);
        }
    }

    // if no valid project was identified, then it is impossible to start the watcher
    if (string.IsNullOrEmpty(projectDirectory)) return;

    // start the watcher process
    var process = Process.Start(new ProcessStartInfo
    {
        FileName = "dotnet",
        Arguments = "build -t:WatchContent --tl:off",
        WorkingDirectory = projectDirectory,
        WindowStyle = ProcessWindowStyle.Normal,
        UseShellExecute = false,
        CreateNoWindow = false
    });

    // when this program exits, make sure to emit a kill signal to the watcher process
    AppDomain.CurrentDomain.ProcessExit += (_, __) =>
    {
        try
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
            }
        }
        catch
        {
            /* ignore */
        }
    };
    AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
    {
        try
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
            }
        }
        catch
        {
            /* ignore */
        }
    };
}