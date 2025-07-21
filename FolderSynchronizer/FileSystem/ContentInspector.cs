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
        var allSourceFiles = _contentManager.GetAllFilesPaths(sourcePath);
        var allReplicaFiles = _contentManager.GetAllFilesPaths(replicaPath);

        for (int i = 0; i < allSourceFiles.Length; i++)
        {
            var sourceFileInfo = new FileInfo(allSourceFiles[i]);
            var replicaFileInfo = new FileInfo(allReplicaFiles[i]);

            if (sourceFileInfo.Length != replicaFileInfo.Length)
                return false;
        }

        return true;
    }

    public bool HasSameFileCount(string sourcePath, string replicaPath)
    {
        var sourceFiles = _contentManager.GetAllFilesPaths(sourcePath);
        var replicaFiles = _contentManager.GetAllFilesPaths(replicaPath);

        return sourceFiles.Length == replicaFiles.Length;
    }

    public bool HasSameDirectoryCount(string sourcePath, string replicaPath)
    {
        var sourceDirectories = _contentManager.GetAllDirectoriesPaths(sourcePath);
        var replicaDirectories = _contentManager.GetAllDirectoriesPaths(replicaPath);

        return sourceDirectories.Length == replicaDirectories.Length;
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