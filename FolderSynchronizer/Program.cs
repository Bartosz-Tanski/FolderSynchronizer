using FolderSynchronizer.FileSystem;
using FolderSynchronizer.UserInteraction;
using FolderSynchronizer.Validators;

namespace FolderSynchronizer;

public static class Program
{
    public static void Main(string[] args)
    {
        var userInterface = new ConsoleUserInterface();
        var monitor = new DirectoryMonitor(new ContentManager(), userInterface, new ContentInspector());
        
        var app = new DirectorySynchronizerApp(monitor, new ArgumentsValidator());

        try
        {
            app.Run(args);
        }
        catch (Exception ex)
        {
            userInterface.DisplayMessage(ex.Message, ConsoleColor.Red);
            userInterface.DisplayHelpMessage();
        }
    }
}