using FolderSynchronizer.FileSystem;
using FolderSynchronizer.UserInteraction;
using FolderSynchronizer.Validators;

namespace FolderSynchronizer;

public static class Program
{
    public static void Main(string[] args)
    {
        var monitor = new DirectoryMonitor(new ContentManager(), new ContentInspector());
        var app = new DirectorySynchronizerApp(monitor, new ArgumentsValidator());

        try
        {
            app.Run(args);
        }
        catch (Exception ex)
        {
            var userInterface = new ConsoleUserInterface();
            userInterface.DisplayMessage(ex.Message, ConsoleColor.Red);
            userInterface.DisplayHelpMessage();
        }
    }
}