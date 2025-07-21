using FolderSynchronizer.Abstractions;

namespace FolderSynchronizer.FileSystem;

public class FilesManager : IFilesManager
{
    public void CopyFile(string sourcePath, string targetPath)
    {
        _userInterface.DisplayMessage($"Copying: {sourcePath} to {targetPath}", ConsoleColor.DarkGray);
        File.Copy(sourcePath, targetPath);
    }
}