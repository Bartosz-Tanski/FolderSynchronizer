using FolderSynchronizer.Abstractions;

namespace FolderSynchronizer.FileSystem;

public class ContentInspector : IContentInspector
{
    public bool HasSameContentSizes(string sourcePath, string replicaPath)
    {
        throw new NotImplementedException();
    }

    public bool HasMatchingTimestamps(string sourcePath, string replicaPath)
    {
        throw new NotImplementedException();
    }

    public bool HasSameContentCount(string sourcePath, string replicaPath)
    {
        throw new NotImplementedException();
    }

    public bool HasMatchingContentStructure(string sourcePath, string replicaPath)
    {
        throw new NotImplementedException();
    }

    public bool IsContentIntegral(string sourcePath, string replicaPath)
    {
        throw new NotImplementedException();
    }
}