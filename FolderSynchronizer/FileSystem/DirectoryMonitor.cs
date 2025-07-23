using FolderSynchronizer.Abstractions;

namespace FolderSynchronizer.FileSystem;

public class DirectoryMonitor : IDirectoryMonitor
{
    private readonly IContentManager _contentManager;
    private readonly IContentInspector _contentInspector;

    public DirectoryMonitor(IContentManager contentManager,
        IContentInspector contentInspector)
    {
        _contentManager = contentManager;
        _contentInspector = contentInspector;
    }

    public void Monitor(string sourcePath, string replicaPath)
    {
        if (!_contentInspector.DoesReplicaDirectoryExist(replicaPath))
        {
            _contentManager.CreateDirectory(replicaPath);
        }

        var sourceDirectories = ReloadDirectories(sourcePath);
        var replicaDirectories = ReloadDirectories(replicaPath);

        var sourceFiles = ReloadFiles(sourcePath);
        var replicaFiles = ReloadFiles(replicaPath);
        
        if (!_contentInspector.HasSameCount(sourceDirectories, replicaDirectories))
        {
            _contentManager.EqualizeDirectoryCount(sourcePath, replicaPath);
            sourceDirectories = ReloadDirectories(sourcePath);
            replicaDirectories = ReloadDirectories(replicaPath);
        }

        if (!_contentInspector.HasSameCount(sourceFiles, replicaFiles))
        {
            _contentManager.EqualizeFileCount(sourcePath, replicaPath);
            sourceFiles = ReloadFiles(sourcePath);
            replicaFiles = ReloadFiles(replicaPath);
        }

        if (!_contentInspector.HasSameFileSizes(sourceFiles, replicaFiles, out var replicaInvalidFileSizes))
        {
            _contentManager.RemoveFiles(replicaInvalidFileSizes);
            sourceFiles = ReloadFiles(sourcePath);
            replicaFiles = ReloadFiles(replicaPath);
        }

        if (!_contentInspector.HasSameNames(sourceFiles, replicaFiles, out var replicaInvalidFileNames))
        {
            _contentManager.RemoveFiles(replicaInvalidFileNames);
            sourceFiles = ReloadFiles(sourcePath);
            replicaFiles = ReloadFiles(replicaPath);
        }

        if (!_contentInspector.HasSameNames(sourceDirectories, replicaDirectories, out _))
        {
            _contentManager.RemoveDirectories(sourcePath, replicaPath);
            sourceFiles = ReloadFiles(sourcePath);
            replicaFiles = ReloadFiles(replicaPath);
        }

        if (!_contentInspector.IsContentIntegral(sourceFiles, replicaFiles, out var notValidReplicaFiles))
        {
            _contentManager.RemoveFiles(notValidReplicaFiles);
        }
        
        _contentManager.EqualizeDirectoryCount(sourcePath, replicaPath);
        _contentManager.EqualizeFileCount(sourcePath, replicaPath);
    }

    private string[] ReloadFiles(string path) => _contentManager.GetAllFilesPaths(path);
    private string[] ReloadDirectories(string path) => _contentManager.GetAllDirectoriesPaths(path);
}