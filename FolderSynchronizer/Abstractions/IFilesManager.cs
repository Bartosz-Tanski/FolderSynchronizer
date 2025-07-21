namespace FolderSynchronizer.Abstractions;

public interface IFilesManager
{
    void CopyFile(string sourcePath, string targetPath);
}