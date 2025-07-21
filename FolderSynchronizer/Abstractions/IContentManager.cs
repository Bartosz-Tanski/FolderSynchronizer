namespace FolderSynchronizer.Abstractions;

public interface IContentManager
{
    string[] GetAllFilesPaths(string path);
    string[] GetAllDirectoriesPaths(string path);

    void CreateDirectories(string sourcePath, string targetPath);
    void RemoveContentSizeMismatch(string sourcePath, string replicaPath);
    void RemoveContentTimestampMismatch(string sourcePath, string replicaPath);
    void EqualizeContentCount(string sourcePath, string replicaPath);
    void RemoveContentStructureMismactch(string sourcePath, string replicaPath);
    void RemoveNonIntegralContent(string sourcePath, string replicaPath);
}