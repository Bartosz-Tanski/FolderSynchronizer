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
            .WriteTo.File(logFilePath, restrictedToMinimumLevel: LogEventLevel.Information, outputTemplate: fileOutputTemplate)
            .CreateLogger();
    }
}