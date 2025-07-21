namespace FolderSynchronizer.Abstractions;

public interface IDirectoriesManager
{
    void CreateDirectories(string sourcePath, string targetPath);
}