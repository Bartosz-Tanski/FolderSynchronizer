using FolderSynchronizer.FileSystem;
using FolderSynchronizer.UserInteraction;
using FolderSynchronizer.Validators;

namespace FolderSynchronizer;

public static class Program
{
    public static void Main(string[] args)
    {
        var userInterface = new ConsoleUserInterface();
        var contentManager = new ContentManager();
        var monitor = new DirectoryMonitor(contentManager, userInterface, new ContentInspector(contentManager));
        
        var app = new DirectorySynchronizerApp(monitor, new ArgumentsValidator(), userInterface);

        try
        {
            app.Run(args);
        }
        catch (Exception ex)
        {
            userInterface.DisplayMessage(ex.Message, ConsoleColor.DarkRed);
            userInterface.DisplayHelpMessage();
        }
    }
}