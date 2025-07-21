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
        
        if (!_contentInspector.HasSameContentSizes(sourcePath, replicaPath))
        {
            _contentManager.RemoveContentSizeMismatch(sourcePath, replicaPath);
            return;
        }
        
        if (!_contentInspector.HasMatchingContentStructure(sourcePath, replicaPath))
        {
            _contentManager.RemoveContentStructureMismatch(sourcePath, replicaPath);
            return;
        }
        
        if (!_contentInspector.IsContentIntegral(sourcePath, replicaPath))
        {
            _contentManager.RemoveNonIntegralContent(sourcePath, replicaPath);
            return;
        }

        _userInterface.DisplayMessage("Directories contains the same files", ConsoleColor.DarkGreen);
    }

    
}