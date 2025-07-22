using FolderSynchronizer.Abstractions;

namespace FolderSynchronizer.FileSystem;

public class DirectoryMonitor : IDirectoryMonitor
{
    private readonly IContentManager _contentManager;
    private readonly IContentInspector _contentInspector;
    private readonly IUserInterface _userInterface;

    public DirectoryMonitor(IContentManager contentManager, 
        IUserInterface userInterface, 
        IContentInspector contentInspector)
    {
        _contentManager = contentManager;
        _userInterface = userInterface;
        _contentInspector = contentInspector;
    }

    public void Monitor(string sourcePath, string replicaPath)
    {
        if (!_contentInspector.HasSameDirectoryCount(sourcePath, replicaPath))
        {
            _contentManager.EqualizeDirectoryCount(sourcePath, replicaPath);
            return;
        }
        
        if (!_contentInspector.HasSameFileCount(sourcePath, replicaPath))
        {
            _contentManager.EqualizeFileCount(sourcePath, replicaPath);
            return;
        }
        
        if (!_contentInspector.HasSameContentSizes(sourcePath, replicaPath, out var replicaFilesWithWrongSizes))
        {
            _contentManager.RemoveFiles(replicaFilesWithWrongSizes);
            return;
        }
        
        if (!_contentInspector.HasSameFilesNames(sourcePath, replicaPath, out var replicaFilesWithWrongNames))
        {
            _contentManager.RemoveFiles(replicaFilesWithWrongNames);
            return;
        }
        
        if (!_contentInspector.IsContentIntegral(sourcePath, replicaPath, out var notValidReplicaFiles))
        {
            _contentManager.RemoveFiles(notValidReplicaFiles);
            return;
        }

        _userInterface.DisplayMessage("Directories are synchronized", ConsoleColor.DarkGreen);
    }

    
}