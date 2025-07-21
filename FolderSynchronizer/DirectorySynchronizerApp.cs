using FolderSynchronizer.Abstractions;

namespace FolderSynchronizer;

public class DirectorySynchronizerApp
{
    private readonly IDirectoryMonitor _monitor;
    private readonly IArgumentsValidator _argumentsValidator;
    private readonly IUserInterface _userInterface;

    private readonly IContentManager _contentManager;

    public DirectorySynchronizerApp(IDirectoryMonitor monitor, IArgumentsValidator argumentsValidator, IUserInterface userInterface, IContentManager contentManager)
    {
        _monitor = monitor;
        _argumentsValidator = argumentsValidator;
        _userInterface = userInterface;
        
        _contentManager = contentManager;
    }

    public void Run(string[] args)
    {
        _argumentsValidator.Validate(args);

        var sourceDirectory = args[0];
        var replicaDirectory = args[1];
        var interval = int.Parse(args[2]);
        var logFilePath = args[3];

        var filesInSource = _contentManager.GetAllFilesPaths(sourceDirectory);
        var filesInReplica = _contentManager.GetAllFilesPaths(replicaDirectory);

        var i = 0;
        Console.WriteLine("Files in source:");
        foreach (var file in filesInSource)
        {
            Console.WriteLine($"{++i}. {file}");
        }

        Console.WriteLine("\n\n");
        
        i = 0;
        Console.WriteLine("Files in replica:");
        foreach (var file in filesInReplica)
        {
            Console.WriteLine($"{++i}. {file}");
        
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
                    _userInterface.DisplayMessage("Press CTRL + C to stop synchronization...", ConsoleColor.Yellow);
                    _monitor.Monitor(sourceDirectory, replicaDirectory);
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
    
    // private void Synchronize(string sourceDirectory, string replicaDirectory)
    // {
    //     var filesInSourceDir = Directory.GetFiles(sourceDirectory);
    //
    //     if (!Directory.Exists(replicaDirectory))
    //     {
    //         _userInterface.DisplayMessage($"Replica directory: {replicaDirectory} doesn't exist.", ConsoleColor.Yellow);
    //         _userInterface.DisplayMessage($"Creating directory at given path: {replicaDirectory} ...", ConsoleColor.DarkGray);
    //
    //         Directory.CreateDirectory(replicaDirectory);
    //
    //         _userInterface.DisplayMessage("Directory created.");
    //     }
    //
    //     foreach (var sourceFilePath in filesInSourceDir)
    //     {
    //         var targetFilePath = Path.Combine(replicaDirectory, Path.GetFileName(sourceFilePath));
    //
    //         // if (!File.Exists(targetFilePath))
    //             // _contentManager.CopyFile(sourceFilePath, targetFilePath);
    //     }
    //
    //     _contentManager.CreateDirectories(sourceDirectory, replicaDirectory);
    //
    //     Console.WriteLine("Press CTRL + C to stop synchronization...");
    // }
}