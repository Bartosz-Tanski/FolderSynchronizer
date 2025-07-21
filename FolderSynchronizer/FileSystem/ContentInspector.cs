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

    public bool HasSameContentCount(string sourcePath, string replicaPath)
    {
        var sourceFiles = _contentManager.GetAllFilesPaths(sourcePath);
        var sourceDirectories = _contentManager.GetAllDirectoriesPaths(sourcePath);
        
        var replicaFiles = _contentManager.GetAllFilesPaths(replicaPath);
        var replicaDirectories = _contentManager.GetAllDirectoriesPaths(replicaPath);

        return sourceFiles.Length + sourceDirectories.Length == replicaFiles.Length + replicaDirectories.Length;
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