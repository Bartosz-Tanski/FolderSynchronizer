using FolderSynchronizer.FileSystem;
using FolderSynchronizer.UserInteraction;
using FolderSynchronizer.Validators;

namespace FolderSynchronizer;

public static class Program
{
    public static void Main(string[] args)
    {
        var app = new DirectorySynchronizerApp(
            new ArgumentsValidator(),
            new ConsoleUserInterface(),
            new DirectoryContentContentManager()
        );

        app.Run(args);
    }
}