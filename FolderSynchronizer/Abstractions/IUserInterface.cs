namespace FolderSynchronizer.Abstractions;

public interface IUserInterface
{
    void DisplayMessage(string message, ConsoleColor? color = null);
    void DisplayHelpMessage();
}