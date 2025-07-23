using FolderSynchronizer.Abstractions;
using FolderSynchronizer.Logger;
using Serilog;

namespace FolderSynchronizer;

public class DirectorySynchronizerApp
{
    private readonly IDirectoryMonitor _monitor;
    private readonly IArgumentsValidator _argumentsValidator;

    public DirectorySynchronizerApp(
        IDirectoryMonitor monitor, 
        IArgumentsValidator argumentsValidator)
    {
        _monitor = monitor;
        _argumentsValidator = argumentsValidator;
    }

    public void Run(string[] args)
    {
        _argumentsValidator.Validate(args);

        var sourceDirectory = args[0];
        var replicaDirectory = args[1];
        var interval = int.Parse(args[2]);
        var logFilePath = args[3];
        
        LoggerConfigurator.Configure(logFilePath);
        
        Log.Verbose("Press CTRL + C to stop synchronization...");
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
                    _monitor.Monitor(sourceDirectory, replicaDirectory);
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, $"Error occured: {ex.Message}");
                    
                    Environment.Exit(1);                
                }
            },
            state: null,
            dueTime: 0, // Synchronizing starts immediately
            period: interval * 1000); // Synchronize each <Interval> seconds

        cancellationToken.Token
            .WaitHandle
            .WaitOne();

        Log.Information("Directory synchronization stopped. (CTRL + C pressed).");
        timer.Dispose();
    }
}