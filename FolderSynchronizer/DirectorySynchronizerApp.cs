using FolderSynchronizer.Abstractions;

namespace FolderSynchronizer;

public class DirectorySynchronizerApp
{
    private readonly IDirectoryMonitor _monitor;
    private readonly IArgumentsValidator _argumentsValidator;
    private readonly IUserInterface _userInterface;

    public DirectorySynchronizerApp(
        IDirectoryMonitor monitor, 
        IArgumentsValidator argumentsValidator, 
        IUserInterface userInterface)
    {
        _monitor = monitor;
        _argumentsValidator = argumentsValidator;
        _userInterface = userInterface;
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
                    _userInterface.DisplayMessage("Press CTRL + C to stop synchronization...", ConsoleColor.Yellow);
                    _monitor.Monitor(sourceDirectory, replicaDirectory);
                }
                catch (Exception ex)
                {
                    _userInterface.DisplayMessage($"Error occured: {ex.Message}", ConsoleColor.DarkRed);
                    _userInterface.DisplayMessage($"For more information check log file: {logFilePath}", ConsoleColor.DarkGray);
                    
                    Environment.Exit(1);                
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
}