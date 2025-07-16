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

            Directory.CreateDirectory(replicaDirectory);

            DisplayMessage("Directory created.");
        }

        var filesInSourceDir = Directory.GetFiles(sourceDirectory);
        var directoriesInSourceDir = Directory.GetDirectories(sourceDirectory);

        foreach (var sourceFilePath in filesInSourceDir)
        {
            var targetFilePath = Path.Combine(replicaDirectory, Path.GetFileName(sourceFilePath));
            
            if (!File.Exists(targetFilePath))
                CopyFile(sourceFilePath, targetFilePath);
            
            DisplayMessage($"File already exists in {targetFilePath}", ConsoleColor.Green);
        }

        foreach (var sourceDirectoryPath in directoriesInSourceDir)
        {
            var targetDirPath = Path.Combine(replicaDirectory, Path.GetFileName(sourceDirectoryPath));

            if (!Directory.Exists(targetDirPath))
                CreateMissingDirectories(sourceDirectoryPath, targetDirPath);
            
            DisplayMessage($"Directory already exists in {targetDirPath}", ConsoleColor.Green);
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        GetDirectories(sourceDirectory);
    }

    private static void DisplayHelpMessage()
    {
        Console.WriteLine("Usage: FolderSynchronizer.exe <SourceDir> <ReplicaDir> <Interval> <LogPath>");
        Console.WriteLine("  Where:");
        Console.WriteLine("    <SourceDir>  - Path to source directory");
        Console.WriteLine("    <ReplicaDir> - Path to replica directory. All files must be the same as in source");
        Console.WriteLine("    <Interval>   - Must be a number. Synchronization interval");
        Console.WriteLine("    <LogPath>    - Path to directory where logs should be stored");

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

    private static void CopyFile(string sourcePath, string targetPath)
    {
        DisplayMessage($"Copying: {sourcePath} to {targetPath}", ConsoleColor.DarkGray);
        File.Copy(sourcePath, targetPath);
    }
    
    private static void CreateMissingDirectories(string sourcePath, string targetPath)
    {
        DisplayMessage($"Copying: {sourcePath} to {targetPath}", ConsoleColor.DarkGray);
        Directory.CreateDirectory(targetPath);
    }

    private static void GetDirectories(string path)
    {
        foreach (var dir in Directory.GetDirectories(path))
        {
            if (Directory.Exists(dir))
            {
                GetDirectories(dir);
                Console.WriteLine("Current dir:" + dir);
            }
        }
    }
}