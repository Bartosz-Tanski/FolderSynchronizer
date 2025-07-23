using Serilog;
using Serilog.Events;

namespace FolderSynchronizer.Logger;

public static class LoggerConfigurator
{
    public static void Configure(string logFilePath)
    {
        const string fileOutputTemplate =
            "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
        
        const string consoleOutputTemplate =
            "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}";

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console(outputTemplate: consoleOutputTemplate)
            .WriteTo.File(
                path: $"{logFilePath}\\sync-.log",
                outputTemplate: fileOutputTemplate,
                restrictedToMinimumLevel: LogEventLevel.Information,
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                retainedFileCountLimit: 30) // Keep files for month to prevent overusing disc free space
            .CreateLogger();
    }
}