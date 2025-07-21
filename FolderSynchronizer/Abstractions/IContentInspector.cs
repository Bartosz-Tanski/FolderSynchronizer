namespace FolderSynchronizer.Abstractions;

public interface IContentInspector
{
    bool HasSameContentSizes(string sourcePath, string replicaPath);
    bool HasSameFileCount(string sourcePath, string replicaPath);
    bool HasSameDirectoryCount(string sourcePath, string replicaPath);
    bool HasMatchingContentStructure(string sourcePath, string replicaPath);
    bool IsContentIntegral(string sourcePath, string replicaPath);
}