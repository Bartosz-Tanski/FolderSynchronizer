namespace FolderSynchronizer.Abstractions;

public interface IContentInspector
{
    bool HasSameContentSizes(string sourcePath, string replicaPath);
    bool HasMatchingTimestamps(string sourcePath, string replicaPath);
    bool HasSameContentCount(string sourcePath, string replicaPath);
    bool HasMatchingContentStructure(string sourcePath, string replicaPath);
    bool IsContentIntegral(string sourcePath, string replicaPath);
}