using FolderSynchronizer.Abstractions;

namespace FolderSynchronizer;

public class DirectorySynchronizerApp
{
    private readonly IArgumentsValidator _argumentsValidator;
    private readonly IUserInterface _userInterface;
    private readonly IDirectoryContentManager _directoryContentManager;

    public DirectorySynchronizerApp(
        IArgumentsValidator argumentsValidator, 
        IUserInterface userInterface, 
        IDirectoryContentManager directoryContentManager)
    {
        _argumentsValidator = argumentsValidator;
        _userInterface = userInterface;
        _directoryContentManager = directoryContentManager;
    }

    public void Run(string[] args)
    {
        _argumentsValidator.Validate(args);

        var sourceDirectory = args[0];
        var replicaDirectory = args[1];
        var interval = int.Parse(args[2]);
        var logFilePath = args[3];

        var dirs = _directoryContentManager.GetAllDirectoriesPaths(sourceDirectory);
        var files = _directoryContentManager.GetAllFilesPaths(sourceDirectory);

        foreach (var dir in dirs)
        {
            Console.WriteLine(dir);
        }

        Console.WriteLine("\n\n");

        foreach (var f in files)
        {
            Console.WriteLine(f);
        }

        // BeginSynchronization(sourceDirectory, replicaDirectory, interval, logFilePath);
    }

    private void BeginSynchronization(string sourceDirectory, string replicaDirectory, int interval, string logFilePath)
    {
        var cancellationToken = new CancellationTokenSource();
        
        Console.CancelKeyPress += (s, e) =>
        {
            e.Cancel = true; // The default is false, which terminates the current process preventing displaying message
            cancellationToken.Cancel();
        };

        var timer = new Timer(_ =>
            {
                try
                {
                    Synchronize(sourceDirectory, replicaDirectory);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"This will be logged: {ex} in {logFilePath}"); // TODO: Implement logging system
                }
            },
            state: null,
            dueTime: 0, // Synchronizing starts immediately
            period: interval * 1000); // Synchronize each <Interval> seconds

        cancellationToken.Token
            .WaitHandle
            .WaitOne();

        _userInterface.DisplayMessage("Directory synchronization stopped. (CTRL + C pressed).", ConsoleColor.Yellow);
        timer.Dispose();
    }
    
    private void Synchronize(string sourceDirectory, string replicaDirectory)
    {
        var filesInSourceDir = Directory.GetFiles(sourceDirectory);

        if (!Directory.Exists(replicaDirectory))
        {
            _userInterface.DisplayMessage($"Replica directory: {replicaDirectory} doesn't exist.", ConsoleColor.Yellow);
            _userInterface.DisplayMessage($"Creating directory at given path: {replicaDirectory} ...", ConsoleColor.DarkGray);

            Directory.CreateDirectory(replicaDirectory);

            _userInterface.DisplayMessage("Directory created.");
        }

        foreach (var sourceFilePath in filesInSourceDir)
        {
            var targetFilePath = Path.Combine(replicaDirectory, Path.GetFileName(sourceFilePath));

            // if (!File.Exists(targetFilePath))
                // _directoryContentManager.CopyFile(sourceFilePath, targetFilePath);
        }

        _directoryContentManager.CreateDirectories(sourceDirectory, replicaDirectory);

        Console.WriteLine("Press CTRL + C to stop synchronization...");
    }
}