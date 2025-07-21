using FolderSynchronizer.Abstractions;

namespace FolderSynchronizer.FileSystem;

public class ContentInspector : IContentInspector
{
    private readonly IContentManager _contentManager;

    public ContentInspector(IContentManager contentManager)
    {
        _contentManager = contentManager;
    }

    public bool HasSameContentSizes(string sourcePath, string replicaPath)
    {
        throw new NotImplementedException();
    }

    public bool HasMatchingTimestamps(string sourcePath, string replicaPath)
    {
        throw new NotImplementedException();
    }

    public bool HasSameFileCount(string sourcePath, string replicaPath)
    {
        var sourceFiles = _contentManager.GetAllFilesPaths(sourcePath);
        var replicaFiles = _contentManager.GetAllFilesPaths(replicaPath);

        return sourceFiles.Length == replicaFiles.Length;
    }

    public bool HasSameDirectoryCount(string sourcePath, string replicaPath)
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