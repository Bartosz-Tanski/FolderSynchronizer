namespace FolderSynchronizer.Abstractions;

public interface IContentManager
{
    string[] GetAllFilesPaths(string path);
    string[] GetAllDirectoriesPaths(string path);
    void EqualizeFileCount(string sourcePath, string replicaPath);
    void EqualizeDirectoryCount(string sourcePath, string replicaPath);
    void RemoveFiles(IEnumerable<string> notValidReplicaFiles);
}