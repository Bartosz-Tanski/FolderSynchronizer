using FolderSynchronizer.Abstractions;

namespace FolderSynchronizer.UserInteraction;

public class ConsoleUserInterface : IUserInterface
{
    public void DisplayMessage(string message, ConsoleColor? color = null)
    {
        var defaultConsoleColor = Console.ForegroundColor;

        if (color is not null)
            Console.ForegroundColor = (ConsoleColor)color;

        Console.WriteLine(message);
        Console.ForegroundColor = defaultConsoleColor;
    }
    
    public void DisplayHelpMessage()
    {
        Console.WriteLine("Usage: FolderSynchronizer.exe <SourceDir> <ReplicaDir> <Interval> <LogPath>");
        Console.WriteLine("  Where:");
        Console.WriteLine("    <SourceDir>  - Path to source directory");
        Console.WriteLine("    <ReplicaDir> - Path to replica directory. All files must be the same as in source");
        Console.WriteLine("    <Interval>   - Must be a number. Synchronization interval in seconds");
        Console.WriteLine("    <LogPath>    - Path to directory where logs should be stored");

        Console.WriteLine();
    }
}