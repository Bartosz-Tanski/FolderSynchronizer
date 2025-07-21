namespace FolderSynchronizer.Abstractions;

public interface IContentManager
{
    string[] GetAllFilesPaths(string path);
    string[] GetAllDirectoriesPaths(string path);
    void RemoveContentSizeMismatch(string sourcePath, string replicaPath);
    void EqualizeFileCount(string sourcePath, string replicaPath);
    void EqualizeDirectoryCount(string sourcePath, string replicaPath);
    void RemoveNonIntegralContent(string sourcePath, string replicaPath);
}