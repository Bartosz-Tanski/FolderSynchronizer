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
            DisplayMessage($"<Interval> argument: {args[2]} is in wrong format!", ConsoleColor.Red);
            DisplayHelpMessage();
            return;
        }

        var sourceDirectory = args[0];
        var replicaDirectory = args[1];
        var interval = int.Parse(args[2]);
        var logFilePath = args[3];

        if (!Directory.Exists(sourceDirectory))
        {
            DisplayMessage($"Source directory: {sourceDirectory} doesn't exist!", ConsoleColor.Red);
            return;
        }
        
        if (!Directory.Exists(replicaDirectory))
        {
            DisplayMessage($"Replica directory: {replicaDirectory} doesn't exist.", ConsoleColor.Yellow);
            DisplayMessage($"Creating directory at given path: {replicaDirectory} ...", ConsoleColor.DarkGray);

            // TODO: Create directory
        }
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
    
    
    private static void DisplayMessage(string message, ConsoleColor? color = null)
    {
        var defaultConsoleColor = Console.ForegroundColor;
        
        if (color is not null)
            Console.ForegroundColor = (ConsoleColor)color;
        
        Console.WriteLine(message);
        Console.ForegroundColor = defaultConsoleColor;
    }
}