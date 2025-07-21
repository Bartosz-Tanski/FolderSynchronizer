namespace FolderSynchronizer.Abstractions;

public interface IDirectoryContentManager
{
    string[] GetAllFilesPaths(string path);
    string[] GetAllDirectoriesPaths(string path);

    void CreateDirectories(string sourcePath, string targetPath);
}