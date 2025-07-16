using FolderSynchronizer.FileSystem;
using FolderSynchronizer.UserInteraction;
using FolderSynchronizer.Validators;

namespace FolderSynchronizer;

public static class Program
{
    public static void Main(string[] args)
    {
        var app = new FolderSynchronizerApp(
            new ArgumentsValidator(),
            new ConsoleUserInterface(),
            new FilesManager(),
            new DirectoriesesManager()
        );

        app.Run(args);
    }
}