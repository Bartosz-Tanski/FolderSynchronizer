using FolderSynchronizer.Abstractions;

namespace FolderSynchronizer;

public class FolderSynchronizerApp
{
    private readonly IArgumentsValidator _argumentsValidator;
    private readonly IUserInterface _userInterface;
    private readonly IFilesManager _filesManager;
    private readonly IDirectoriesManager _directoriesManager;

    public FolderSynchronizerApp(
        IArgumentsValidator argumentsValidator, 
        IUserInterface userInterface, 
        IFilesManager filesManager, 
        IDirectoriesManager directoriesManager)
    {
        _argumentsValidator = argumentsValidator;
        _userInterface = userInterface;
        _filesManager = filesManager;
        _directoriesManager = directoriesManager;
    }

    public void Run(string[] args)
    {
        _argumentsValidator.Validate(args);

        var sourceDirectory = args[0];
        var replicaDirectory = args[1];
        var interval = int.Parse(args[2]);
        var logFilePath = args[3];

        BeginSynchronization(sourceDirectory, replicaDirectory, interval, logFilePath);
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

            if (!File.Exists(targetFilePath))
                CopyFile(sourceFilePath, targetFilePath);
        }

        CreateDirectories(sourceDirectory, replicaDirectory);

        Console.WriteLine("Press CTRL + C to stop synchronization...");
    }
    
    private void CopyFile(string sourcePath, string targetPath)
    {
        _userInterface.DisplayMessage($"Copying: {sourcePath} to {targetPath}", ConsoleColor.DarkGray);
        File.Copy(sourcePath, targetPath);
    }

    private void CreateDirectories(string sourcePath, string targetPath)
    {
        var innerSourceDirectories = Directory.GetDirectories(sourcePath);

        foreach (var innerDirectory in innerSourceDirectories)
        {
            var combinedTargetPath = Path.Combine(targetPath, Path.GetFileName(innerDirectory));

            if (!Directory.Exists(combinedTargetPath))
            {
                _userInterface.DisplayMessage($"Creating folder at: {combinedTargetPath}", ConsoleColor.DarkGray);
                Directory.CreateDirectory(combinedTargetPath);
            }

            var innerSourceFiles = Directory.GetFiles(innerDirectory);
            if (innerSourceFiles.Length > 0)
            {
                foreach (var innerSourceFile in innerSourceFiles)
                {
                    var combinedTargetPathForFile = Path.Combine(combinedTargetPath, Path.GetFileName(innerSourceFile));

                    if (!File.Exists(combinedTargetPathForFile))
                    {
                        CopyFile(innerSourceFile, combinedTargetPathForFile);
                    }
                }
            }

            if (Directory.GetDirectories(innerDirectory).Length > 0)
            {
                CreateDirectories(innerDirectory, combinedTargetPath);
            }
        }
    }
}