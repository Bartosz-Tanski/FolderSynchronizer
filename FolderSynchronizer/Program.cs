namespace FolderSynchronizer;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 4)
        {
            DisplayHelpMessage();
            return;
        }

        if (!int.TryParse(args[2], out _))
        {
            DisplayMessage(ConsoleColor.Red, $"<Interval> argument: {args[2]} is in wrong format!");
            DisplayHelpMessage();
            return;
        }

        var sourceDirectory = args[0];
        var replicaDirectory = args[1];
        var interval = int.Parse(args[2]);
        var logFilePath = args[3];

    }
    
    private static void DisplayHelpMessage()
    {
        Console.WriteLine("Usage: FolderSynchronizer.exe <SourceDir> <ReplicaDir> <Interval> <LogPath>");
        Console.WriteLine("  Where:");
        Console.WriteLine("    <SourceDir>  - Path to source directory");
        Console.WriteLine("    <ReplicaDir> - Path to replica directory. All files must be the same as in source");
        Console.WriteLine("    <Interval>   - Must be a number. Determines after what time content in folders is checked. ");
        Console.WriteLine("    <LogPath>    - Path to directory where logs should be stored.");

        Console.WriteLine();
    }
    
    private static void DisplayMessage(ConsoleColor color, string message)
    {
        var defaultConsoleColor = Console.ForegroundColor;

        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = defaultConsoleColor;
    }
}