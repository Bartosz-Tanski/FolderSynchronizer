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
        Console.WriteLine("    <SourceDir>  - Path to the source directory whose contents you want to replicate.");
        Console.WriteLine("    <ReplicaDir> - Path to the replica directory. All files and subfolders will mirror the source.");
        Console.WriteLine("    <Interval>   - Synchronization interval in seconds. Must be a positive integer (e.g. 30).");
        Console.WriteLine("    <LogPath>    - Path to the directory where synchronization logs will be stored.");

        Console.WriteLine();
    }
}