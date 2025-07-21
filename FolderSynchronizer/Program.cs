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
        
        var app = new DirectorySynchronizerApp(monitor, new ArgumentsValidator(), userInterface, new ContentManager());

        app.Run(args);
    }
}